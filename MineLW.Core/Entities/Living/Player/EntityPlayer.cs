using System;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Utils;

namespace MineLW.Entities.Living.Player
{
    public class EntityPlayer : EntityLiving, IEntityPlayer
    {
        public PlayerProfile Profile { get; }
        public GameMode GameMode { get; set; }

        private IClient _client;

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