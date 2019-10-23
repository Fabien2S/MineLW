using System;

namespace MineLW.API.Worlds.Chunks.Generator
{
    public interface IChunkGenerator
    {
        void Generate(ChunkPosition position, IChunk chunk, Random random);
    }
}