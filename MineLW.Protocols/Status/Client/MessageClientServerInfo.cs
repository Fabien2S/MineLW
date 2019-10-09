using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Protocols.Status.Client
{
    public class MessageClientServerInfo : MessageSerializer<MessageClientServerInfo.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteJson(message.Status);
        }

        public struct Message : IMessage
        {
            public readonly ServerStatus Status;
            public Message(ServerStatus status)
            {
                Status = status;
            }
        }
    }
}