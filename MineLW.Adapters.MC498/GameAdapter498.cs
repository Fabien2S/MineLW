using System.Collections.Immutable;
using MineLW.Adapters.MC498.Networking;
using MineLW.API;
using MineLW.API.Blocks.Properties;
using MineLW.API.Client;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Registries;
using MineLW.API.Utils;
using MineLW.Blocks;
using MineLW.Entities;
using MineLW.Networking;

namespace MineLW.Adapters.MC498
{
    public class GameAdapter498 : IGameAdapter
    {
        public GameVersion Version { get; } = new GameVersion("1.14.4", 498);

        public NetworkState NetworkState { get; } = new GameState();
        
        public IBlockRegistry BlockRegistry { get; } = new BlockRegistry();
        public IEntityRegistry EntityRegistry { get; } = new EntityRegistry();

        public GameAdapter498()
        {
            BlockRegistry.Register(
                Minecraft.CreateIdentifier("air"),
                ImmutableArray<IBlockProperty>.Empty,
                ImmutableList<dynamic>.Empty
            );
            BlockRegistry.Register(
                Minecraft.CreateIdentifier("stone"),
                ImmutableArray<IBlockProperty>.Empty,
                ImmutableList<dynamic>.Empty
            );
            BlockRegistry.Register(
                Minecraft.CreateIdentifier("granite"),
                ImmutableArray<IBlockProperty>.Empty,
                ImmutableList<dynamic>.Empty
            );
            
            EntityRegistry.Register<IEntityPlayer>(Minecraft.Entities.Player);
        }

        public IClientConnection CreateConnection(NetworkClient networkClient) => new ClientConnection(networkClient);
    }
}