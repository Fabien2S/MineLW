using MineLW.Adapters.MC498.Networking;
using MineLW.API.Blocks;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Registries;
using MineLW.API.Utils;
using MineLW.Networking;
using GameClient = MineLW.Adapters.Networking.GameClient;

namespace MineLW.Adapters.MC498
{
    public class GameAdapter498 : IGameAdapter
    {
        public GameVersion Version { get; } = new GameVersion("1.14.4", 498);
        public NetworkState NetworkState { get; } = new GameState();
        public Registry<Identifier, IBlock> Blocks { get; } = new Registry<Identifier, IBlock>();
        public Registry<int, IBlockState> BlockStates { get; } = new Registry<int, IBlockState>();

        public GameClient CreateClient(PlayerProfile profile, NetworkClient client)
        {
            return new Networking.GameClient(profile, client);
        }
    }
}