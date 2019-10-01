namespace MineLW.Networking
{
    public class NetworkController
    {
        protected readonly NetworkConnection Connection;

        protected NetworkController(NetworkConnection connection)
        {
            Connection = connection;
        }
    }
}