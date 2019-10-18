using MineLW.API.Blocks;
using MineLW.API.Registries;
using MineLW.API.Utils;

namespace MineLW.Adapters.Blocks
{
    public interface IBlockManager
    {
        Registry<int, IBlockState> BlockStates { get; }

        IBlockState CreateState(int id);
        IBlock GetBlock(Identifier name);
    }
}