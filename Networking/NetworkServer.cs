﻿using System;
using System.Collections.Generic;
using System.Net;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using MineLW.API.Utils;
using MineLW.Debugging;
using MineLW.Networking.Handlers;
using MineLW.Networking.Handshake;

namespace MineLW.Networking
{
    public class NetworkServer : ChannelInitializer<TcpSocketChannel>, IUpdatable
    {
        private const int ReadIdleTimeout = 20;
        private const int WriteIdleTimeout = 15;

        private static readonly Logger Logger = LogManager.GetLogger<NetworkServer>();
        
        private readonly ServerBootstrap _bootstrap = new ServerBootstrap();
        private readonly HashSet<NetworkClient> _clients = new HashSet<NetworkClient>();
        
        private readonly HandshakeAdapter _handshakeAdapter = new HandshakeAdapter();
        private readonly Dictionary<uint, NetworkAdapter> _adapters = new Dictionary<uint, NetworkAdapter>();

        private IEventLoopGroup _bossGroup;
        private IEventLoopGroup _workerGroup;

        public void Update(float deltaTime)
        {
            _clients.RemoveWhere(client => !client.IsConnected);
            foreach (var client in _clients)
                client.Update(deltaTime);
        }

        protected override void InitChannel(TcpSocketChannel channel)
        {
            Logger.Debug("Connection from {0}", channel.RemoteAddress);
            channel.Configuration.SetOption(ChannelOption.TcpNodelay, true);

            var client = new NetworkClient
            {
                Adapter = _handshakeAdapter
            };

            channel.Pipeline
                .AddLast("idle_timeout", new IdleStateHandler(
                    ReadIdleTimeout,
                    WriteIdleTimeout,
                    0
                ))
                .AddLast(MessageFramingHandler.Name, new MessageFramingHandler())
                .AddLast(MessageEncodingHandler.Name, new MessageEncodingHandler(client))
                .AddLast(NetworkClient.Name, client);

            _clients.Add(client);
        }

        public void Start(EndPoint endPoint)
        {
            Logger.Debug("Starting network server on {0}", endPoint);
            
            _bossGroup = new MultithreadEventLoopGroup();
            _workerGroup = new MultithreadEventLoopGroup();

            try
            {
                _bootstrap.Group(_bossGroup, _workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    .ChildOption(ChannelOption.TcpNodelay, true)
                    .ChildOption(ChannelOption.SoKeepalive, true)
                    .ChildHandler(this)
                    .BindAsync(endPoint)
                    .ContinueWith(task =>
                    {
                        if (task.IsCompletedSuccessfully)
                        {
                            Logger.Success("Network server started on {0}", endPoint);
                        }
                        else
                        {
                            Logger.Error("Unable to start the network server: {0}", task.Exception);
                            Environment.Exit(1);
                        }
                    });
            }
            finally
            {
                _bossGroup.ShutdownGracefullyAsync();
                _workerGroup.ShutdownGracefullyAsync();
            }
        }

        public void Stop()
        {
            Logger.Debug("Stopping network server...");
            _bootstrap.Group().ShutdownGracefullyAsync();
            _bootstrap.ChildGroup().ShutdownGracefullyAsync();
        }
    }
}