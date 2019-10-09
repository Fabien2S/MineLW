using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Utils;

namespace MineLW.Entities.Living.Player
{
    public class EntityPlayer : EntityLiving, IEntityPlayer
    {
        public PlayerProfile Profile { get; }

        private IClient _client;

        public EntityPlayer(IClient client = null)
        {
            if (client != null)
            {
                Profile = client.Profile;
            }

            _client = client;
        }
    }
}