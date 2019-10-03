using MineLW.Debugging;
using MineLW.Networking.Messages;

namespace MineLW.Networking.Handshake
{
    public class HandshakeController : MessageController
    {
        private static readonly Logger Logger = LogManager.GetLogger<HandshakeController>();

        public HandshakeController(NetworkClient client) : base(client)
        {
        }

        public void HandleHandshake(HandshakeMessage.Message message)
        {
            Logger.Debug("Handshake received from {1} ({0})", message.Protocol, Client);
        }
    }
}