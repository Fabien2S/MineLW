using System;
using DotNetty.Buffers;
using MineLW.API.Client;
using MineLW.API.Client.World;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.Clients.World;
using MineLW.Networking.IO;

namespace MineLW.Clients
{
    public class Client : IClient
    {
        private const double KeepAliveDelay = 15;
        private const double TimedOutDelay = 30;
        
        public PlayerProfile Profile { get; }
        public IClientConnection Connection { get; }
        public IClientController Controller { get; }

        public IEntityPlayer Player { get; private set; }
        public IClientWorld World { get; }
        
        public float Latency { get; private set; }

        // ping
        private float _sincePingRequest;
        private uint _pingRequestId;
        private bool _waitingForPingResponse;

        public Client(IClientConnection connection, IClientController controller, PlayerProfile profile)
        {
            Profile = profile;
            Connection = connection;
            Controller = controller;
            
            World = new ClientWorld(this);
        }

        public void Init(IEntityPlayer player)
        {
            if (Player != null)
            {
                Connection.Disconnect();
                return;
            }

            Player = player;
            World.Init();

            Controller.Client = this;
            Controller.Disconnected += OnDisconnect;
            Controller.PingResponseReceived += OnPingResponse;
            Connection.Spawn(this, player);
        }

        private void OnDisconnect(object sender, TextComponent e)
        {
            Player.Remove();
        }

        public void SendCustom(Identifier channel, Action<IByteBuffer> serializer)
        {
            var buffer = Unpooled.Buffer();
            serializer(buffer);

            var data = buffer.ReadBytes();
            Connection.SendCustom(channel, data);
        }

        public void Update(float deltaTime)
        {
            World.Update(deltaTime);

            _sincePingRequest += deltaTime;
            if (_waitingForPingResponse)
            {
                if (_sincePingRequest < TimedOutDelay)
                    return;

                _sincePingRequest = 0;
                Connection.Disconnect(new TextComponentTranslate("disconnect.timeout"));
            }
            else if (_sincePingRequest >= KeepAliveDelay)
            {
                _sincePingRequest = 0;
                _waitingForPingResponse = true;
                Connection.SendPingRequest(_pingRequestId);
            }
        }

        private void OnPingResponse(object sender, long id)
        {
            if(!_waitingForPingResponse)
                return;
            
            if (_pingRequestId != id)
                return;

            Latency = _sincePingRequest;
            _pingRequestId++;
            _sincePingRequest = 0;
            _waitingForPingResponse = false;
        }
    }
}