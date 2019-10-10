using DotNetty.Buffers;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.Networking;

namespace MineLW.Adapters.Networking
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

        public void Update(float deltaTime)
        {
        }

        public abstract void Init(IEntityPlayer player);
        public abstract void SendCustom(Identifier channel, IByteBuffer buffer);
        public abstract void SendMessage(TextComponent message);
        public abstract void Disconnect(TextComponentString reason);

        public sealed override string ToString()
        {
            return Profile.Name + '[' + NetworkClient + ']';
        }
    }
}