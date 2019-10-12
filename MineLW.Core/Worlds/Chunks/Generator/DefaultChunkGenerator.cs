using MineLW.Adapters;
using MineLW.API;
using MineLW.API.Blocks;
using MineLW.API.Worlds.Chunks;
using MineLW.API.Worlds.Chunks.Generator;

namespace MineLW.Worlds.Chunks.Generator
{
    public class DefaultChunkGenerator : IChunkGenerator
    {
        private readonly IBlockState _stoneBlockState;
        
        public DefaultChunkGenerator()
        {
            var serverAdapter = GameAdapter.ServerAdapter;
            var stoneBlockName = Minecraft.CreateIdentifier("stone");
            var stoneBlock = serverAdapter.Blocks[stoneBlockName];
            _stoneBlockState = serverAdapter.BlockStates[stoneBlock.Id];
        }

        public void Generate(IChunk chunk)
        {
            for (var x = 0; x < Minecraft.Units.Chunk.Size; x++)
            for (var z = 0; z < Minecraft.Units.Chunk.Size; z++)
                chunk.SetBlock(x, 0, z, _stoneBlockState);
        }
    }
}