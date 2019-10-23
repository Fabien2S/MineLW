using DotNetty.Buffers;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientPingRequest : MessageSerializer<MessageClientPingRequest.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteLong(message.PingId);
        }

        public struct Message : IMessage
        {
            public readonly long PingId;

            public Message(long pingId)
            {
                PingId = pingId;
            }
        }
    }
}