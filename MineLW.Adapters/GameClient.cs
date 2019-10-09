using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.Networking;

namespace MineLW.Adapters
{
    public abstract class GameClient : IClient
    {
        public PlayerProfile Profile { get; }
        protected readonly NetworkClient NetworkClient;

        protected GameClient(PlayerProfile profile, NetworkClient networkClient)
        {
            Profile = profile;
            NetworkClient = networkClient;
        }

        public abstract void Init(IEntityPlayer player);
        public abstract void SendMessage(TextComponent message);
        public abstract void Kick(TextComponentString reason);

        public sealed override string ToString()
        {
            return Profile.Name + '[' + NetworkClient + ']';
        }
    }
}