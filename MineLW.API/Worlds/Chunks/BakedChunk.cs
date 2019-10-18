using MineLW.API.Blocks;

namespace MineLW.API.Worlds.Chunks
{
    public class BakedChunk
    {
        private readonly IBlockState[,,] _blockStates = new IBlockState[
            Minecraft.Units.Chunk.Size,
            Minecraft.Units.Chunk.Height,
            Minecraft.Units.Chunk.Size
        ];

        public bool HasBlock(int x, int y, int z)
        {
            return _blockStates[x, y, z] != null;
        }

        public void SetBlock(int x, int y, int z, IBlockState blockState)
        {
            _blockStates[x, y, z] = blockState;
        }

        public IBlockState GetBlock(int x, int y, int z)
        {
            return _blockStates[x, y, z];
        }
    }
}