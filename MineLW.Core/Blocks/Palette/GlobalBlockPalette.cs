using System;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Palette;
using MineLW.API.Registries;

namespace MineLW.Blocks.Palette
{
    public class GlobalBlockPalette : IBlockPalette
    {
        public byte BitsPerBlock { get; }

        private readonly Registry<int, IBlockState> _registry;

        public GlobalBlockPalette(Registry<int, IBlockState> registry)
        {
            _registry = registry;

            BitsPerBlock = (byte) Math.Ceiling(
                Math.Log(registry.Count) / Math.Log(2)
            );
        }

        public int GetId(IBlockState blockState)
        {
            var type = blockState.Type;
            var properties = type.Properties;
            var data = 0;

            for (var i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                var value = blockState.Properties[i];
                var index = property.GetIndex(value);

                data *= property.ValueCount;
                data += index;
            }

            return type.Id + data;
        }

        public IBlockState GetBlockState(int id)
        {
            return _registry[id];
        }
    }
}