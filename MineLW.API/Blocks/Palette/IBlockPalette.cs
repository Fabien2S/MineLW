namespace MineLW.API.Blocks.Palette
{
    public interface IBlockPalette
    {
        byte BitsPerBlock { get; }

        int GetId(IBlockState blockState);
        IBlockState GetBlockState(int id);
    }
}