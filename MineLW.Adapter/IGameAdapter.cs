using MineLW.API.Client;
using MineLW.API.Utils;
using MineLW.Networking;

namespace MineLW.Adapter
{
    public interface IGameAdapter
    {
        GameVersion Version { get; }

        IClient CreateClient(PlayerProfile profile, NetworkClient client);
    }
}