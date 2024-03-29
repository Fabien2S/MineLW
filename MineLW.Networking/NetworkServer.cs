﻿using System.Collections.Generic;
using System.Net;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using MineLW.API.Server;
using MineLW.API.Utils;
using MineLW.Networking.Handlers;
using NLog;

namespace MineLW.Networking
{
    public class NetworkServer : ChannelInitializer<TcpSocketChannel>, IUpdatable
    {
        private const int ReadIdleTimeout = 30;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IServer _server;
        private readonly NetworkState _defaultState;
        private readonly ServerBootstrap _bootstrap = new ServerBootstrap();
        private readonly HashSet<NetworkClient> _clients = new HashSet<NetworkClient>();

        private IEventLoopGroup _eventLoopGroup;

        public NetworkServer(IServer server, NetworkState defaultState)
        {
            _server = server;
            _defaultState = defaultState;
        }

        public void Update(float deltaTime)
        {
            _clients.RemoveWhere(client => client.Closed);
            foreach (var client in _clients)
                client.Update(deltaTime);
        }

        public void Start(EndPoint endPoint)
        {
            _eventLoopGroup = new MultithreadEventLoopGroup(CreateEventLoop);

            _bootstrap
                .Group(_eventLoopGroup)
                .Channel<TcpServerSocketChannel>()
                .ChildOption(ChannelOption.TcpNodelay, true)
                .ChildOption(ChannelOption.SoKeepalive, true)
                .ChildHandler(this)
                .BindAsync(endPoint)
                .ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                        Logger.Info("Network server started on {0}", endPoint);
                    else
                    {
                        Logger.Error("Unable to start the network server: {0}", task.Exception);
                        _server.Shutdown();
                    }
                });
        }

        public void Stop()
        {
            Logger.Debug("Stopping network server...");
            _eventLoopGroup.ShutdownGracefullyAsync();
        }

        protected override void InitChannel(TcpSocketChannel channel)
        {
            Logger.Debug("Connection from {0}", channel.RemoteAddress);

            var client = new NetworkClient(_server)
            {
                State = _defaultState
            };

            channel.Pipeline
                .AddLast("timeout", new ReadTimeoutHandler(ReadIdleTimeout))
                .AddLast(MessageFramingHandler.Name, new MessageFramingHandler())
                .AddLast(MessageEncodingHandler.Name, new MessageEncodingHandler(client))
                .AddLast(NetworkClient.Name, client);

            _clients.Add(client);
        }

        private static IEventLoop CreateEventLoop(IEventLoopGroup group)
        {
            return new SingleThreadEventLoop(group, "DotNetty");
        }
    }
}