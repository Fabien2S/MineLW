using MineLW.API.Utils;
using MineLW.Networking;

namespace MineLW.Adapters
{
    public interface IGameAdapter
    {
        GameVersion Version { get; }
        NetworkState NetworkState { get; }

        GameClient CreateClient(PlayerProfile profile, NetworkClient client);
    }
}