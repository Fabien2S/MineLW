using System;
using System.Collections.Generic;
using MineLW.API.Blocks;
using MineLW.API.Extensions;
using MineLW.API.Math;
using MineLW.API.Registries;
using MineLW.API.Utils;

namespace MineLW.Blocks
{
    public class BlockManager : IBlockManager
    {
        public byte BitsPerBlock => (byte) MathF.Ceiling(
            MathF.Log(_blockStates.Count) / MathHelper.Log2
        );

        private readonly Registry<Identifier, IBlock> _blocks = new Registry<Identifier, IBlock>();
        private readonly Registry<int, IBlockState> _blockStates = new Registry<int, IBlockState>();

        public void Register(IBlock block)
        {
            _blocks[block.Name] = block;

            for (var state = 0; state < block.StateCount; state++)
            {
                var properties = block.Properties;
                var propertyCount = properties.Count;

                var data = state;
                var props = new dynamic[propertyCount];
                for (var j = propertyCount - 1; j >= 0; j--)
                {
                    var property = properties[j];

                    var propertySize = property.ValueCount;
                    var valueIndex = data % propertySize;
                    data /= propertySize;

                    var value = property.GetValue(valueIndex);
                    props[j] = value;
                }

                var stateId = block.Id + state;
                _blockStates[stateId] = new BlockState(stateId, block, props);
            }
        }

        public IBlockState CreateState(Identifier name, Dictionary<string, string> properties = null)
        {
            var block = _blocks[name];

            var blockDefaultValues = block.DefaultValues;
            var blockProperties = block.Properties;

            if (properties == null || properties.Count == 0)
                return _blockStates[block.Id]; // return the default state, which is block id + block data of 0

            var props = new dynamic[blockProperties.Count];
            for (var i = 0; i < blockProperties.Count; i++)
            {
                var blockProperty = blockProperties[i];
                var blockPropertyName = blockProperty.Name;
                if (properties.ContainsKey(blockPropertyName))
                {
                    var propValue = properties[blockPropertyName];
                    props[i] = blockProperty.Parse(propValue);
                }
                else
                    props[i] = blockDefaultValues[i];
            }

            var id = block.GetStateId(props);
            return _blockStates[id];
        }

        public IBlockState this[int id] => _blockStates[id];
        public IBlock this[Identifier name] => _blocks[name];
    }
}