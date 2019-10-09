using DotNetty.Buffers;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Protocols.Status.Server
{
    public class MessageServerPing : MessageDeserializer<StatusController, MessageServerPing.Message>
    {
        public override IMessage Deserialize(IByteBuffer buffer)
        {
            return new Message(
                buffer.ReadLong()
            );
        }

        protected override void Handle(StatusController controller, Message message)
        {
            controller.HandlePing(message.Payload);
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