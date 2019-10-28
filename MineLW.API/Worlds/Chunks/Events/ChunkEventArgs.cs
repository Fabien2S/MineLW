using System;

namespace MineLW.API.Worlds.Chunks.Events
{
    public class ChunkEventArgs : EventArgs
    {
        public readonly ChunkPosition Position;
        public readonly IChunk Chunk;

        public ChunkEventArgs(ChunkPosition position, IChunk chunk)
        {
            Position = position;
            Chunk = chunk;
        }
    }
}