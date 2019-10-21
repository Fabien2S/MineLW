using System.Collections.Generic;
using System.Linq;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Properties;
using MineLW.API.Extensions;
using MineLW.API.Registries;
using MineLW.API.Utils;
using NLog;

namespace MineLW.Blocks
{
    public class BlockManager : IBlockManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // TODO replace BitsPerBlock with the global palette one when all the blocks are implemented
        public byte BitsPerBlock => 14;
        //public byte BitsPerBlock => (byte) MathF.Ceiling(MathF.Log2(_blockStates.Count));

        private readonly Registry<Identifier, IBlock> _blocks = new Registry<Identifier, IBlock>();
        private readonly Registry<int, IBlockState> _blockStates = new Registry<int, IBlockState>();

        public void Register(Identifier name, IReadOnlyList<IBlockProperty> properties, IReadOnlyList<dynamic> defaultValues)
        {
            var blockId = _blockStates.Count;
            var block = new Block(blockId, name, properties, defaultValues);
            _blocks[name] = block;

            Logger.Debug("Registering block {0}: {1}", blockId, block);
            var stateCount = properties.Aggregate(1, (current, property) => current * property.ValueCount);
            for (var blockData = 0; blockData < stateCount; blockData++)
            {
                var propertyCount = properties.Count;

                var tmp = blockData;
                var props = new dynamic[propertyCount];
                for (var j = propertyCount - 1; j >= 0; j--)
                {
                    var property = properties[j];

                    var propertySize = property.ValueCount;
                    var valueIndex = tmp % propertySize;
                    tmp /= propertySize;

                    var value = property.GetValue(valueIndex);
                    props[j] = value;
                }

                var stateId = blockId + blockData;
                _blockStates[stateId] = new BlockState(stateId, block, props);
                Logger.Debug("Registering block state {0}: {1}", stateId, _blockStates[stateId]);
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