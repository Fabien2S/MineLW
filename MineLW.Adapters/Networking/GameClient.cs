using DotNetty.Buffers;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Text;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.Networking;

namespace MineLW.Adapters.Networking
{
    public abstract class GameClient : IClient
    {
        public PlayerProfile Profile { get; }
        public IEntityPlayer Player { get; private set; }
        
        public IClientWorld World { get; set; }
        
        protected readonly NetworkClient NetworkClient;

        protected GameClient(NetworkClient networkClient, PlayerProfile profile)
        {
            Profile = profile;
            NetworkClient = networkClient;
        }

        public void Update(float deltaTime)
        {
        }

        public void Init(IEntityPlayer player)
        {
            if(Player != null)
            {
                NetworkClient.Disconnect();
                return;
            }

            Player = player;
            _Init(player);
        }
        
        protected abstract void _Init(IEntityPlayer player);
        public abstract void SendCustom(Identifier channel, IByteBuffer buffer);
        public abstract void SendMessage(TextComponent message);
        public abstract void Disconnect(TextComponentString reason);
        public abstract void Respawn(IWorldContext worldContext);

        public sealed override string ToString()
        {
            return Profile.Name + '[' + NetworkClient + ']';
        }
    }
}