using System;
using System.Collections.ObjectModel;
using MineLW.API.Blocks.Properties;
using MineLW.API.Utils;

namespace MineLW.API.Blocks
{
    public interface IBlock : IEquatable<IBlock>
    {
        int Id { get; }
        Identifier Name { get; }
        ReadOnlyCollection<IBlockProperty> Properties { get; }
        int StateCount { get; }

        IBlockState CreateDefaultState();
        IBlockState CreateState(int blockData);
    }
}