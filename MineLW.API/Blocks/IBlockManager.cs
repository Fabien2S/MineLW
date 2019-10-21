using MineLW.API.Registries;
using MineLW.API.Utils;

namespace MineLW.API.Blocks
{
    public interface IBlockManager
    {
        Registry<int, IBlockState> BlockStates { get; }

        IBlockState CreateState(int id);
        IBlock this[Identifier stone] { get; }
    }
}