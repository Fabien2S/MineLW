using DotNetty.Buffers;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Protocols.Status.Client
{
    public class MessageClientPong : MessageSerializer<MessageClientPong.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteLong(message.Payload);
        }

        public struct Message : IMessage
        {
            public readonly long Payload;

            public Message(long payload)
            {
                Payload = payload;
            }
        }
    }
}