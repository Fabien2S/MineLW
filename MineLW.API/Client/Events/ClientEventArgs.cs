using System;

namespace MineLW.API.Client.Events
{
    public class ClientEventArgs : EventArgs
    {
        public readonly IClient Client;

        public ClientEventArgs(IClient client)
        {
            Client = client;
        }
    }
}