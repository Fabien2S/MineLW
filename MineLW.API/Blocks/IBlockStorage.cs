namespace MineLW.API.Blocks
{
    public interface IBlockStorage
    {
        bool HasBlock(int x, int y, int z);
        void SetBlock(int x, int y, int z, IBlockState blockState);
        IBlockState GetBlock(int x, int y, int z);
    }
}