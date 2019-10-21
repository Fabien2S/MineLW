using System.Collections.Generic;
using MineLW.API.Blocks.Properties;
using MineLW.API.Utils;

namespace MineLW.API.Blocks
{
    public interface IBlockManager
    {
        byte BitsPerBlock { get; }

        /*Registry<Identifier, IBlock> Blocks { get; }
        Registry<int, IBlockState> BlockStates { get; }*/

        void Register(Identifier name, IReadOnlyList<IBlockProperty> properties, IReadOnlyList<dynamic> defaultValues);
        IBlockState CreateState(Identifier name, Dictionary<string, string> properties = null);

        IBlockState this[int id] { get; }
        IBlock this[Identifier name] { get; }
    }
}