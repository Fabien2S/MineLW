using DotNetty.Buffers;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Palette;
using MineLW.API.Extensions;

namespace MineLW.Blocks.Palette
{
    public class GlobalBlockPalette : IBlockPalette
    {
        public byte BitsPerBlock { get; }

        private readonly IBlockManager _blockManager;

        public GlobalBlockPalette(IBlockManager blockManager)
        {
            _blockManager = blockManager;
            BitsPerBlock = blockManager.BitsPerBlock;
        }

        public int GetId(IBlockState blockState)
        {
            var type = blockState.Block;
            return type.GetStateId(blockState.Properties);
        }

        public IBlockState GetBlockState(int id)
        {
            return _blockManager[id];
        }

        public void Serialize(IByteBuffer buffer)
        {
        }
    }
}