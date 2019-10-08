using MineLW.API.Client;

namespace MineLW.Networking.States.Game
{
    public abstract class GameState : NetworkState
    {
        public abstract IClient CreateClient(NetworkClient networkClient);
    }
}