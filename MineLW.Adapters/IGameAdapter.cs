using MineLW.API.Blocks;
using MineLW.API.Client;
using MineLW.API.Registries;
using MineLW.API.Utils;
using MineLW.Networking;

namespace MineLW.Adapters
{
    public interface IGameAdapter
    {
        GameVersion Version { get; }

        NetworkState NetworkState { get; }
        IBlockRegistry BlockRegistry { get; }

        IClientConnection CreateConnection(NetworkClient networkClient);
    }
}