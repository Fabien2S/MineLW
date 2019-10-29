using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientEntityLook : MessageSerializer<MessageClientEntityLook.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteVarInt32(message.Id);
            buffer.WriteByte((int) (message.Yaw / 360 * 256));
        }
        
        public struct Message : IMessage
        {
            public readonly int Id;
            public readonly float Yaw;

            public Message(int id, float yaw)
            {
                Id = id;
                Yaw = yaw;
            }
        }
    }
}