using System;
using MineLW.API.Text;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking.States.Handshake
{
    public class HandshakeState : NetworkState
    {
        public static readonly HandshakeState Instance = new HandshakeState();
        
        private HandshakeState() : base(true)
        {
        }

        public override IMessage CreateDisconnectMessage(TextComponent reason)
        {
            return null;
        }

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

        protected internal override MessageController CreateController(NetworkClient client)
        {
            return new HandshakeController(client);
        }
    }
}