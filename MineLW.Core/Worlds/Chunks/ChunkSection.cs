using MineLW.Adapters;
using MineLW.API.Blocks;
using MineLW.API.Worlds.Chunks;

namespace MineLW.Worlds.Chunks
{
    public class ChunkSection : IChunkSection
    {
        public const int BlockCount = Chunk.Size * Chunk.SectionCount * Chunk.Size;

        public IBlockStorage BlockStorage { get; }

        public ChunkSection()
        {
            var serverAdapter = GameAdapter.ServerAdapter;
            BlockStorage = serverAdapter.CreateBlockStorage();
        }

        public bool HasBlock(int x, int y, int z)
        {
            return BlockStorage.HasBlock(x, y, z);
        }

        public void SetBlock(int x, int y, int z, IBlockState blockState)
        {
            BlockStorage.SetBlock(x, y, z, blockState);
        }

        public IBlockState GetBlock(int x, int y, int z)
        {
            return BlockStorage.GetBlock(x, y, z);
        }
    }
}