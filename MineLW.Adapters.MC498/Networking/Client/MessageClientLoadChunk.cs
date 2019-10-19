using DotNetty.Buffers;
using MineLW.API.NBT;
using MineLW.API.Utils;
using MineLW.API.Worlds.Chunks;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientLoadChunk : MessageSerializer<MessageClientLoadChunk.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteChunkPosition(message.ChunkPosition);
            buffer.WriteBoolean(message.FullChunk);
            buffer.WriteVarInt32(message.SectionMask);
            
            buffer.WriteCompound(new NbtCompound
            {
                ["MOTION_BLOCKING"] = new NbtLongArray(message.HeightMap.Backing),
                ["WORLD_SURFACE"] = new NbtLongArray(message.HeightMap.Backing)
            });

            buffer.WriteVarInt32(message.Data.ReadableBytes);
            buffer.WriteBytes(message.Data);
            
            buffer.WriteVarInt32(0); // tile entities
        }
        
        public struct Message : IMessage
        {
            public readonly ChunkPosition ChunkPosition;
            public readonly bool FullChunk;
            public readonly int SectionMask;
            public readonly NBitsArray HeightMap;
            public readonly IByteBuffer Data;

            public Message(ChunkPosition chunkPosition, bool fullChunk, int sectionMask, NBitsArray heightMap, IByteBuffer data)
            {
                ChunkPosition = chunkPosition;
                FullChunk = fullChunk;
                SectionMask = sectionMask;
                HeightMap = heightMap;
                Data = data;
            }
        }
    }
}