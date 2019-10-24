using System;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Entities.Living.Player.Events;

namespace MineLW.Entities.Living.Player
{
    public class EntityPlayer : EntityLiving, IEntityPlayer
    {
        public PlayerProfile Profile { get; }

        public PlayerMode PlayerMode
        {
            get => _playerMode;
            set
            {
                EnsureValid();
                
                var eventArgs = new PlayerGameModeEventArgs(this, _playerMode, value);
                GameModeChanged?.Invoke(this, eventArgs);
                if(!eventArgs.Cancelled)
                    _playerMode = value;
            }
        }

        public event EventHandler<PlayerGameModeEventArgs> GameModeChanged;

        private IClient _client;

        private PlayerMode _playerMode;

        public EntityPlayer(int id, Guid uuid) : base(id, uuid)
        {
            Profile = new PlayerProfile(
                uuid,
                "NPC"
            );
        }

        public EntityPlayer(int id, IClient client) : this(id, client.Profile.Id)
        {
            Profile = client.Profile;
            _client = client;
        }
    }
}