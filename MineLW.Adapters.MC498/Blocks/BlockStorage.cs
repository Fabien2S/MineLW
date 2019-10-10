using System;
using MineLW.API.Blocks;

namespace MineLW.Adapters.MC498.Blocks
{
    public class BlockStorage : IBlockStorage
    {
        public bool HasBlock(int x, int y, int z)
        {
            throw new NotImplementedException();
        }

        public void SetBlock(int x, int y, int z, IBlockState blockState)
        {
            throw new NotImplementedException();
        }

        public IBlockState GetBlock(int x, int y, int z)
        {
            throw new NotImplementedException();
        }

        private static int Index(int x, int y, int z)
        {
            return (y & 15) << 8 | (z & 15) << 4 | x & 15;
        }
    }
}