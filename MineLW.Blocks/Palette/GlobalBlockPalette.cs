using DotNetty.Buffers;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Palette;
using MineLW.API.Extensions;
using MineLW.API.Registries;

namespace MineLW.Blocks.Palette
{
    public class GlobalBlockPalette : IBlockPalette
    {
        public byte BitsPerBlock { get; }

        private readonly IBlockRegistry _blockRegistry;

        public GlobalBlockPalette(IBlockRegistry blockRegistry)
        {
            _blockRegistry = blockRegistry;
            BitsPerBlock = blockRegistry.BitsPerBlock;
        }

        public int GetId(IBlockState blockState)
        {
            var type = blockState.Block;
            return type.GetStateId(blockState.Properties);
        }

        public IBlockState GetBlockState(int id)
        {
            return _blockRegistry[id];
        }

        public void Serialize(IByteBuffer buffer)
        {
        }
    }
}