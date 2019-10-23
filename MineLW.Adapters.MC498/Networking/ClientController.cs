using System;
using System.Numerics;
using MineLW.API.Client;
using MineLW.API.Entities.Events;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Math;
using MineLW.Networking;
using MineLW.Networking.Messages;

namespace MineLW.Adapters.MC498.Networking
{
    public class ClientController : MessageController, IClientController
    {
        public event EventHandler<EntityPositionChangedEventArgs> PositionChanged;

        private IEntityPlayer _player;
        
        public ClientController(NetworkClient client) : base(client)
        {
        }
        
        public void Init(IEntityPlayer player)
        {
            if(_player != null)
                throw new InvalidOperationException("Player already set");
            
            _player = player;
        }

        public void HandlePlayerPositionUpdate(Vector3 position)
        {
            var from = _player.Position;
            PositionChanged?.Invoke(this, new EntityPositionChangedEventArgs(_player, from, position));
        }

        public void HandlePlayerRotationUpdate(Rotation rotation)
        {
        }

        public void HandlePlayerGroundedUpdate(in bool grounded)
        {
        }

        public void HandleTeleportConfirm(in int teleportId)
        {
        }
    }
}