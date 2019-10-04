using System;
using MineLW.Debugging;
using MineLW.Networking.Messages;
using MineLW.Networking.States.Status;

namespace MineLW.Networking.States.Handshake
{
    public class HandshakeController : MessageController
    {
        private static readonly Logger Logger = LogManager.GetLogger<HandshakeController>();

        public HandshakeController(NetworkClient client) : base(client)
        {
        }

        public void HandleHandshake(HandshakeMessage.Message message)
        {
            Logger.Debug("Handshake received from {1} with state {2} ({0})", message.Protocol, Client, message.RequestedState);

            switch (message.RequestedState)
            {
                case 1:
                    Client.State = new StatusState();
                    break;
                default:
                    throw new NotSupportedException("Invalid requested state: " + message.RequestedState);
            }
        }
    }
}