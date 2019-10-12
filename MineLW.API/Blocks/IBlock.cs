using System.Collections.ObjectModel;
using MineLW.API.Blocks.Properties;
using MineLW.API.Utils;

namespace MineLW.API.Blocks
{
    public interface IBlock
    {
        int Id { get; }
        Identifier Name { get; }
        ReadOnlyCollection<IBlockProperty> Properties { get; }
        int StateCount { get; }
    }
}