using System;
using MineLW.API;
using MineLW.API.Blocks;
using MineLW.API.Worlds.Chunks;
using MineLW.API.Worlds.Chunks.Generator;

namespace MineLW.Worlds.Chunks.Generator
{
    public class DefaultChunkGenerator : IChunkGenerator
    {
        private readonly IBlockState _blockState;
        
        public DefaultChunkGenerator(IBlockState blockState)
        {
            _blockState = blockState;
        }

        public void Generate(ChunkPosition position, IChunk chunk, Random random)
        {
            for (var x = 0; x < Minecraft.Units.Chunk.Size; x++)
            for (var z = 0; z < Minecraft.Units.Chunk.Size; z++)
                chunk.SetBlock(x, 0, z, _blockState);
        }
    }
}