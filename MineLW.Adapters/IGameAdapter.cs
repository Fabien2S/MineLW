using MineLW.Adapters.Networking;
using MineLW.API.Blocks;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Registries;
using MineLW.API.Utils;
using MineLW.Networking;

namespace MineLW.Adapters
{
    public interface IGameAdapter
    {
        GameVersion Version { get; }

        NetworkState NetworkState { get; }
        Registry<Identifier, IBlock> Blocks { get; }
        Registry<int, IBlockState> BlockStates { get; }

        GameClient CreateClient(NetworkClient client, PlayerProfile profile);
    }
}