using System;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Entities.Living.Player.Events;
using MineLW.API.Utils;

namespace MineLW.Entities.Living.Player
{
    public class EntityPlayer : EntityLiving, IEntityPlayer
    {
        public PlayerProfile Profile { get; }

        public GameMode GameMode
        {
            get => _gameMode;
            set
            {
                var eventArgs = new PlayerGameModeEventArgs(this, _gameMode, value);
                GameModeChanged?.Invoke(this, eventArgs);
                if(!eventArgs.Canceled)
                    _gameMode = value;
            }
        }

        public event EventHandler<PlayerGameModeEventArgs> GameModeChanged;

        private IClient _client;

        private GameMode _gameMode;

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