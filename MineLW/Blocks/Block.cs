using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ReadOnlyCollection<IBlockProperty> Properties { get; }
        
        public readonly int StateCount;

        internal readonly object[] DefaultValues;

        public Block(int id, Identifier name, IList<IBlockProperty> properties, object[] defaultValues)
        {
            Id = id;
            Name = name;

            Properties = new ReadOnlyCollection<IBlockProperty>(properties);
            DefaultValues = defaultValues;

            StateCount = properties.Aggregate(1, (current, property) => current * property.GetValueCount());
        }

        public override string ToString()
        {
            return Name.ToString();
        }

        public BlockState CreateState(int blockData)
        {
            var propertyCount = Properties.Count;

            var props = new object[propertyCount];
            for (var i = propertyCount - 1; i >= 0; i--)
            {
                var property = Properties[i];

                var propertySize = property.GetValueCount();
                var valueIndex = blockData % propertySize;
                blockData /= propertySize;

                var value = property.GetValue(valueIndex);
                props[i] = value;
            }

            return new BlockState(Id, this, props);
        }

        protected bool Equals(Block other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is Block other && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}