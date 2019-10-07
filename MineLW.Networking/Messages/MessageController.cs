namespace MineLW.Networking.Messages
{
    public abstract class MessageController
    {
        protected readonly NetworkClient Client;

        protected MessageController(NetworkClient client)
        {
            Client = client;
        }
    }
}