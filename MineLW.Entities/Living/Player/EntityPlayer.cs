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
                
                var previousGameMode = _gameMode;
                
                var gameModeCancelEventArgs = new GameModeCancelEventArgs(previousGameMode, value);
                GameModeChanging?.Invoke(this, gameModeCancelEventArgs);
                if (gameModeCancelEventArgs.Cancel)
                    return;
                
                _gameMode = value;

                var gameModeEventArgs = new GameModeEventArgs(previousGameMode, value);
                GameModeChanged?.Invoke(this, gameModeEventArgs);
            }
        }

        public event EventHandler<GameModeCancelEventArgs> GameModeChanging;
        public event EventHandler<GameModeEventArgs> GameModeChanged;

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