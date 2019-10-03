using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking.Handshake
{
    public class HandshakeState : NetworkState
    {
        protected override IMessageSerializer[] GetSerializers()
        {
            return new IMessageSerializer[0];
        }

        protected override IMessageDeserializer[] GetDeserializers()
        {
            return new IMessageDeserializer[]
            {
                new HandshakeMessage()
            };
        }

        public override MessageController CreateController(NetworkClient client)
        {
            return new HandshakeController(client);
        }
    }
}