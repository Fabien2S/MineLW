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

        private readonly IBlockPalette _globalPalette;

        private int[] _globalBlockStateIds;
        private int _position;

        public LinearBlockPalette(IBlockPalette globalPalette, byte bitsPerBlock)
        {
            _globalPalette = globalPalette;

            BitsPerBlock = bitsPerBlock;
            _globalBlockStateIds = new int[1 << BitsPerBlock];

            // store "air"
            _position = 1;
            _globalBlockStateIds[0] = 0;
        }

        public void Resize(byte bitsPerBlock)
        {
            var length = 1 << bitsPerBlock;
            if (_globalBlockStateIds.Length == length)
                return;

            Array.Resize(ref _globalBlockStateIds, length);
            BitsPerBlock = bitsPerBlock;
        }

        public int GetId(IBlockState blockState)
        {
            var id = _globalPalette.GetId(blockState);
            if (id == -1)
                throw new InvalidOperationException("Block not registered inside the global palette");

            var index = Array.IndexOf(_globalBlockStateIds, id, 0, _position);
            if (index != -1)
                return index;

            if (_position >= _globalBlockStateIds.Length)
                return -1;

            _globalBlockStateIds[_position] = id;
            return _position++;
        }

        public IBlockState GetBlockState(int id)
        {
            if (0 > id || id >= _position)
                return null;

            var blockStateId = _globalBlockStateIds[id];
            return _globalPalette.GetBlockState(blockStateId);
        }

        public void Serialize(IByteBuffer buffer)
        {
            buffer.WriteVarInt32(_position);
            for (var i = 0; i < _position; i++)
                buffer.WriteVarInt32(_globalBlockStateIds[i]);
        }
    }
}