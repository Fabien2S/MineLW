using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking.States.Handshake
{
    public class HandshakeMessage : MessageDeserializer<HandshakeController, HandshakeMessage.Message>
    {
        public override IMessage Deserialize(IByteBuffer buffer)
        {
            return new Message(
                buffer.ReadVarInt32(),
                buffer.ReadUtf8(),
                buffer.ReadUnsignedShort(),
                buffer.ReadVarInt32()
            );
        }

        protected override void Handle(HandshakeController controller, Message message)
        {
            controller.HandleHandshake(message);
        }

        public struct Message : IMessage
        {
            public readonly int Protocol;
            public readonly string IpAddress;
            public readonly ushort Port;
            public readonly int RequestedState;

            public Message(int protocol, string ipAddress, ushort port, int requestedState)
            {
                Protocol = protocol;
                IpAddress = ipAddress;
                Port = port;
                RequestedState = requestedState;
            }
        }
    }
}