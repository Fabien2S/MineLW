namespace MineLW.Networking.Messages
{
    public abstract class MessageController
    {
        protected readonly NetworkClient NetworkClient;

        protected MessageController(NetworkClient networkClient)
        {
            NetworkClient = networkClient;
        }
    }
}