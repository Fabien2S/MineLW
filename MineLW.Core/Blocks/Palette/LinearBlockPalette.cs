using System;
using DotNetty.Buffers;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Palette;
using MineLW.Networking.IO;

namespace MineLW.Blocks.Palette
{
    public class LinearBlockPalette : IBlockPalette
    {
        public byte BitsPerBlock { get; private set; }

        private readonly BlockStorage _blockStorage;
        private readonly IBlockPalette _globalPalette;

        private int[] _globalBlockStateIds;

        private int _highestIndex;

        public LinearBlockPalette(BlockStorage blockStorage, IBlockPalette globalPalette, byte bitsPerBlock)
        {
            _blockStorage = blockStorage;
            _globalPalette = globalPalette;

            BitsPerBlock = bitsPerBlock;
            _globalBlockStateIds = new int[1 << BitsPerBlock];

            // store "air"
            _highestIndex = 1;
        }

        private void UpdateArray(byte bitsPerBlock)
        {
            var length = 1 << bitsPerBlock;
            if (_globalBlockStateIds.Length == length)
                return;

            Array.Resize(ref _globalBlockStateIds, length);
            BitsPerBlock = bitsPerBlock;
        }

        public int GetId(IBlockState blockState)
        {
            var blockStateId = _globalPalette.GetId(blockState);

            var index = Array.IndexOf(_globalBlockStateIds, blockStateId, 0, _highestIndex);
            if (index != -1)
                return index;

            var id = _highestIndex;
            if (_highestIndex < _globalBlockStateIds.Length)
            {
                _globalBlockStateIds[id] = blockStateId;
                _highestIndex++;
                return id;
            }

            var bitsPerBlock = (byte) (BitsPerBlock + 1);
            _blockStorage.Resize(bitsPerBlock);
            UpdateArray(bitsPerBlock);

            var blockPalette = _blockStorage.BlockPalette;
            return blockPalette.GetId(blockState);
        }

        public IBlockState GetBlockState(int id)
        {
            if (0 > id || id >= _highestIndex)
                return BlockState.Air;

            var blockStateId = _globalBlockStateIds[id];
            return _globalPalette.GetBlockState(blockStateId);
        }

        public void Serialize(IByteBuffer buffer)
        {
            buffer.WriteVarInt32(_highestIndex);
            for (var i = 0; i < _highestIndex; i++)
                buffer.WriteVarInt32(_globalBlockStateIds[i]);
        }
    }
}