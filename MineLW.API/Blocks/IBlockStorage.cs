using MineLW.API.Blocks.Palette;
using MineLW.API.IO;

namespace MineLW.API.Blocks
{
    public interface IBlockStorage : IByteBufferSerializable
    {
        IBlockPalette BlockPalette { get; }
        ushort BlockCount { get; }

        bool HasBlock(int x, int y, int z);
        void SetBlock(int x, int y, int z, IBlockState blockState);
        IBlockState GetBlock(int x, int y, int z);
    }
}