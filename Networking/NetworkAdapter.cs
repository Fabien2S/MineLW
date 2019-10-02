namespace MineLW.Networking
{
    public abstract class NetworkAdapter
    {
        public abstract NetworkController CreateController(NetworkState state, NetworkClient client);
    }
}