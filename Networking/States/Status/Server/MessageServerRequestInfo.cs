using DotNetty.Buffers;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking.States.Status.Server
{
    public class MessageServerRequestInfo : MessageDeserializer<StatusController, MessageServerRequestInfo.Message>
    {
        public override IMessage Deserialize(IByteBuffer buffer)
        {
            return new Message();
        }

        public override void Handle(StatusController controller, Message message)
        {
            controller.HandleInfoRequest();
        }

        public struct Message : IMessage
        {
        }
    }
}