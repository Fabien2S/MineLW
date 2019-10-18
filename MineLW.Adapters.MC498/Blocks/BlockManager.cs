using System;
using MineLW.Adapters.Blocks;
using MineLW.API.Blocks;
using MineLW.API.Registries;
using MineLW.API.Utils;

namespace MineLW.Adapters.MC498.Blocks
{
    public class BlockManager : IBlockManager
    {
        public Registry<int, IBlockState> BlockStates { get; }

        private readonly IBlock[] _blocks = new IBlock[0];

        public IBlockState CreateState(int id)
        {
            throw new NotImplementedException();
        }

        public IBlock GetBlock(Identifier name)
        {
            throw new NotImplementedException();
        }
    }
}