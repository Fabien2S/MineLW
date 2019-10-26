using System;
using System.Globalization;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Entities.Living.Player.Events;

namespace MineLW.Entities.Living.Player
{
    public class EntityPlayer : EntityLiving, IEntityPlayer
    {
        public PlayerProfile Profile { get; }
        
        public bool HasClient { get; }
        public IClient Client { get; }

        public bool IsListed { get; set; } = true;
        
        public GameMode GameMode
        {
            get => _gameMode;
            set
            {
                EnsureValid();
                
                var eventArgs = new PlayerGameModeEventArgs(this, _gameMode, value);
                GameModeChanged?.Invoke(this, eventArgs);
                if(!eventArgs.Cancelled)
                    _gameMode = value;
            }
        }

        public event EventHandler<PlayerGameModeEventArgs> GameModeChanged;

        private GameMode _gameMode;

        public EntityPlayer(int id, Guid uuid) : base(id, uuid)
        {
            Profile = new PlayerProfile(
                uuid,
                id.ToString(NumberFormatInfo.InvariantInfo),
                new PlayerProfile.Property[0]
            );
            HasClient = false;
        }

        public EntityPlayer(int id, PlayerProfile profile) : base(id, profile.Id)
        {
            Profile = profile;
            HasClient = false;
        }

        public EntityPlayer(int id, IClient client) : this(id, client.Profile.Id)
        {
            Profile = client.Profile;
            HasClient = true;
            Client = client;
        }
    }
}