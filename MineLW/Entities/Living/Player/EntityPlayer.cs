using MineLW.API.Entities.Living.Player;
using MineLW.API.Utils;
using MineLW.Client;

namespace MineLW.Entities.Living.Player
{
    public class EntityPlayer : EntityLiving, IEntityPlayer
    {
        public PlayerProfile Profile { get; }

        private GameClient _client;

        public EntityPlayer(GameClient client = null)
        {
            if (client != null)
            {
                var networkClient = client.NetworkClient;
                Profile = networkClient.Profile;
            }

            _client = client;
        }
    }
}