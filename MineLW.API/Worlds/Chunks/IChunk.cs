using MineLW.API.Blocks;

namespace MineLW.API.Worlds.Chunks
{
    public interface IChunk
    {
        bool HasSection(int index);
        IChunkSection CreateSection(int index);
        void RemoveSection(int index);

        bool HasBlock(int x, int y, int z);
        void SetBlock(int x, int y, int z, IBlockState blockState);
        IBlockState GetBlock(int x, int y, int z);

        IChunkSection this[int index] { get; }
    }
}