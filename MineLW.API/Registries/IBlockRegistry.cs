using System.Collections.Generic;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Properties;
using MineLW.API.Utils;

namespace MineLW.API.Registries
{
    public interface IBlockRegistry
    {
        byte BitsPerBlock { get; }

        void Register(Identifier name, IReadOnlyList<IBlockProperty> properties, IReadOnlyList<dynamic> defaultValues);
        IBlockState CreateState(Identifier name, Dictionary<string, string> properties = null);

        IBlockState this[int id] { get; }
        IBlock this[Identifier name] { get; }
    }
}