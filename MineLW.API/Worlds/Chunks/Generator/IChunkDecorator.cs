using System;

namespace MineLW.API.Worlds.Chunks.Generator
{
    public interface IChunkDecorator
    {
        void Decorate(IChunk chunk, Random random);
    }
}