using MineLW.Adapters.Blocks;
using MineLW.Adapters.MC498.Blocks;
using MineLW.Adapters.MC498.Networking;
using MineLW.API.Client;
using MineLW.API.Utils;
using MineLW.Networking;

namespace MineLW.Adapters.MC498
{
    public class GameAdapter498 : IGameAdapter
    {
        public GameVersion Version { get; } = new GameVersion("1.14.4", 498);
        public NetworkState NetworkState { get; } = new GameState();
        public IBlockManager BlockManager { get; } = new BlockManager();

        public IClientConnection CreateConnection(NetworkClient networkClient)
        {
            return new ClientConnection(networkClient);
        }
    }
}