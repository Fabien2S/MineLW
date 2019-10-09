using MineLW.Adapter.MC498.Networking;
using MineLW.API.Client;
using MineLW.API.Utils;
using MineLW.Networking;

namespace MineLW.Adapter.MC498
{
    public class GameAdapter498 : IGameAdapter
    {
        public GameVersion Version { get; } = new GameVersion("1.14.4", 498);
        public NetworkState NetworkState { get; } = new GameState498();

        public IClient CreateClient(PlayerProfile profile, NetworkClient client)
        {
            return new GameClient498(profile, client);
        }
    }
}