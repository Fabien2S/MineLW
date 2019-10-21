using System.Collections.Generic;
using System.Linq;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Properties;
using MineLW.API.Utils;

namespace MineLW.Blocks
{
    public class Block : IBlock
    {
        public int Id { get; }
        public Identifier Name { get; }
        public int StateCount { get; }
        public IReadOnlyList<IBlockProperty> Properties { get; }
        public IReadOnlyList<dynamic> DefaultValues { get; }

        public Block(int id, Identifier name, IReadOnlyList<IBlockProperty> properties,
            IReadOnlyList<dynamic> defaultValues)
        {
            Id = id;
            Name = name;

            Properties = properties;
            DefaultValues = defaultValues;

            StateCount = properties.Aggregate(1, (current, property) => current * property.ValueCount);
        }

        public bool Equals(IBlock other)
        {
            return Id == other?.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is IBlock other && other.Id == Id;
        }

        public override int GetHashCode() => Id;
        public override string ToString() => Name.ToString();
    }
}