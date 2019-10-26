using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientDestroyEntities : MessageSerializer<MessageClientDestroyEntities.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteVarInt32(message.Ids.Length);
            foreach (var id in message.Ids)
                buffer.WriteVarInt32(id);
        }

        public struct Message : IMessage
        {
            public readonly int[] Ids;

            public Message(int[] ids)
            {
                Ids = ids;
            }
        }
    }
}