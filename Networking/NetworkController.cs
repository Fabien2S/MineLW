namespace MineLW.Networking
{
    public class NetworkController
    {
        protected readonly NetworkClient Client;

        protected NetworkController(NetworkClient client)
        {
            Client = client;
        }
    }
}