using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking.Handshake
{
    public class HandshakeMessage : MessageDeserializer<HandshakeMessage.Message>
    {
        public HandshakeMessage() : base(0x00)
        {
        }

        public override Message Deserialize(IByteBuffer buffer)
        {
            return new Message(
                buffer.ReadVarInt32(),
                buffer.ReadUtf8(),
                buffer.ReadUnsignedShort(),
                buffer.ReadVarInt32()
            );
        }

        public struct Message
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