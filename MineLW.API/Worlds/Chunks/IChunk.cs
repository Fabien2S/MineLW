using MineLW.API.Blocks;
using MineLW.API.Collections;

namespace MineLW.API.Worlds.Chunks
{
    public interface IChunk
    {
        NBitsArray HeightMap { get; }

        bool HasSection(int index);
        IChunkSection CreateSection(int index);
        void RemoveSection(int index);

        bool HasBlock(int x, int y, int z);
        IBlockState GetBlock(int x, int y, int z);
        void SetBlock(int x, int y, int z, IBlockState blockState);

        IChunkSection this[int index] { get; }
    }
}