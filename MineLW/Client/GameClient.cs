using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.Networking;

namespace MineLW.Client
{
    public abstract class GameClient : IClient
    {
        protected internal readonly NetworkClient NetworkClient;

        protected GameClient(NetworkClient networkClient)
        {
            NetworkClient = networkClient;
        }

        protected internal abstract void InitGame(IEntityPlayer player);

        public abstract void SendMessage(TextComponent message);
        public abstract void Kick(TextComponentString reason);
    }
}