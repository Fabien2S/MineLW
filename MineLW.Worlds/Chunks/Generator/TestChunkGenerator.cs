
using MineLW.API;
using MineLW.API.Blocks;
using MineLW.API.Worlds.Chunks;
using MineLW.API.Worlds.Chunks.Generator;

namespace MineLW.Worlds.Chunks.Generator
{
    public class TestChunkGenerator : IChunkGenerator
    {
        private readonly IBlockState _blockState;
        
        public TestChunkGenerator(IBlockState blockState)
        {
            _blockState = blockState;
        }

        public void Generate(IChunk chunk)
        {
            for (var x = 0; x < Minecraft.Units.Chunk.Size; x+=4)
            for (var z = 0; z < Minecraft.Units.Chunk.Size; z+=4)
                chunk.SetBlock(x, 1, z, _blockState);
        }
    }
}