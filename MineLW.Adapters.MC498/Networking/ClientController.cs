using System;
using System.Numerics;
using MineLW.API.Client;
using MineLW.API.Math;
using MineLW.Networking;
using MineLW.Networking.Messages;

namespace MineLW.Adapters.MC498.Networking
{
    public class ClientController : MessageController, IClientController
    {
        public IClient Client
        {
            get => _client;
            set
            {
                if(_client != null)
                    throw new InvalidOperationException("Controller already init");
                _client = value;
            }
        }
        
        public event EventHandler<Vector3> PositionChanged;
        public event EventHandler<int> TeleportConfirmed;
        public event EventHandler<long> PingResponseReceived;

        private IClient _client;
        
        public ClientController(NetworkClient networkClient) : base(networkClient)
        {
        }

        public void HandlePlayerPositionUpdate(in Vector3 position)
        {
            PositionChanged?.Invoke(this, position);
        }

        public void HandlePlayerRotationUpdate(in Rotation rotation)
        {
        }

        public void HandlePlayerGroundedUpdate(in bool grounded)
        {
        }

        public void HandleTeleportConfirm(in int id)
        {
            TeleportConfirmed?.Invoke(this, id);
        }

        public void HandlePingResponse(in long pingId)
        {
            PingResponseReceived?.Invoke(this, pingId);
        }
    }
}