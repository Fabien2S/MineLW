using System.Collections.Immutable;
using MineLW.Adapters.MC498.Networking;
using MineLW.API;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Properties;
using MineLW.API.Client;
using MineLW.API.Utils;
using MineLW.Networking;

namespace MineLW.Adapters.MC498
{
    public class GameAdapter498 : IGameAdapter
    {
        public GameVersion Version { get; } = new GameVersion("1.14.4", 498);

        public NetworkState NetworkState { get; } = new GameState();
        public IBlockManager BlockManager { get; } = GameAdapters.BlockManagerSupplier();

        public GameAdapter498()
        {
            BlockManager.Register(
                Minecraft.CreateIdentifier("air"),
                ImmutableArray<IBlockProperty>.Empty,
                ImmutableList<dynamic>.Empty
            );
            BlockManager.Register(
                Minecraft.CreateIdentifier("stone"),
                ImmutableArray<IBlockProperty>.Empty,
                ImmutableList<dynamic>.Empty
            );
        }

        public IClientConnection CreateConnection(NetworkClient networkClient) => new ClientConnection(networkClient);
    }
}