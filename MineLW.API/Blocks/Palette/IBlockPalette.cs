using MineLW.API.IO;

namespace MineLW.API.Blocks.Palette
{
    public interface IBlockPalette : IByteBufferSerializable
    {
        byte BitsPerBlock { get; }

        int GetId(IBlockState blockState);
        IBlockState GetBlockState(int id);
    }
}