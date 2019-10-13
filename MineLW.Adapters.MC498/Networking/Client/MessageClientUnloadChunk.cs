using DotNetty.Buffers;
using MineLW.API.Worlds.Chunks;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientUnloadChunk : MessageSerializer<MessageClientUnloadChunk.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteChunkPosition(message.ChunkPosition);
        }
        
        public struct Message : IMessage
        {
            public readonly ChunkPosition ChunkPosition;

            public Message(ChunkPosition chunkPosition)
            {
                ChunkPosition = chunkPosition;
            }
        }
    }
}