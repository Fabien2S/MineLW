using DotNetty.Buffers;
using MineLW.API.Worlds.Chunks;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientUpdateViewPosition : MessageSerializer<MessageClientUpdateViewPosition.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteVarInt32(message.Position.X);
            buffer.WriteVarInt32(message.Position.Z);
        }

        public struct Message : IMessage
        {
            public readonly ChunkPosition Position;

            public Message(ChunkPosition position)
            {
                Position = position;
            }
        }
    }
}