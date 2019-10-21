using System.Collections.Generic;
using MineLW.API.Blocks;

namespace MineLW.API.Extensions
{
    public static class BlockExtensions
    {
        public static int GetStateId(this IBlock block, IReadOnlyList<dynamic> prop)
        {
            var properties = block.Properties;
            var data = 0;

            for (var i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var value = prop[i];
                var index = (int) property.GetIndex(value);

                data *= property.ValueCount;
                data += index;
            }

            return block.Id + data;
        }

        /*public static IBlockState CreateState(this IBlock block, int blockData)
        {
            var properties = block.Properties;
            var propertyCount = properties.Count;

            var props = new object[propertyCount];
            for (var i = propertyCount - 1; i >= 0; i--)
            {
                var property = properties[i];

                var propertySize = property.ValueCount;
                var valueIndex = blockData % propertySize;
                blockData /= propertySize;

                var value = property.GetValue(valueIndex);
                props[i] = value;
            }

            return new BlockState(Id, this, props);
        }*/
    }
}