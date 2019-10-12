using System;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Palette;
using MineLW.API.Utils;
using MineLW.Blocks.Palette;
using MineLW.Utils;

namespace MineLW.Blocks
{
    public class BlockStorage : IBlockStorage
    {
        private const byte MinBitsPerBlock = 4;

        public IBlockPalette BlockPalette { get; private set; }

        public ushort BlockCount
        {
            get
            {
                if (_blockCount != ushort.MaxValue)
                    return _blockCount;

                _blockCount = 0;
                var capacity = NBitsArray.Capacity;
                for (var i = 0; i < capacity; i++)
                {
                    if (NBitsArray[i] != 0)
                        _blockCount++;
                }

                return _blockCount;
            }
        }

        public NBitsArray NBitsArray { get; private set; }

        private readonly IBlockPalette _globalPalette;
        private ushort _blockCount = ushort.MaxValue;

        public BlockStorage(IBlockPalette globalPalette, byte bitsPerBlock = MinBitsPerBlock)
        {
            _globalPalette = globalPalette;
            BlockPalette = _globalPalette;
            UpdatePalette(bitsPerBlock);
        }

        /// <summary>
        /// Update the internal palette to match the bitsPerBlock
        /// </summary>
        /// <param name="bitsPerBlock">The number of bits used to store a block state</param>
        /// <returns>true if the internal palette was updated, false otherwise</returns>
        private void UpdatePalette(byte bitsPerBlock)
        {
            bitsPerBlock = Math.Max(bitsPerBlock, MinBitsPerBlock);

            if (BlockPalette != null && BlockPalette.BitsPerBlock == bitsPerBlock)
                return;

            if (bitsPerBlock <= 8)
            {
                if (!(BlockPalette is LinearBlockPalette))
                    BlockPalette = new LinearBlockPalette(this, _globalPalette, bitsPerBlock);
            }
            else
                BlockPalette = _globalPalette;

            NBitsArray = NBitsArray.Create(bitsPerBlock, 4096);
        }

        public void Resize(byte bitsPerBlock)
        {
            if (bitsPerBlock > 32)
                throw new ArgumentOutOfRangeException(nameof(bitsPerBlock), "bitsPerBlock > 32");

            var previousDataBits = NBitsArray;
            var previousBlockPalette = BlockPalette;

            UpdatePalette(bitsPerBlock);

            for (var i = 0; i < previousDataBits.Capacity; i++)
            {
                var blockStateId = previousDataBits[i];
                if (blockStateId == 0)
                    continue;

                var blockState = previousBlockPalette.GetBlockState(blockStateId);
                NBitsArray[i] = BlockPalette.GetId(blockState);
            }
        }

        public bool HasBlock(int x, int y, int z)
        {
            var index = Index(x, y, z);
            return NBitsArray[index] != 0;
        }

        public void SetBlock(int x, int y, int z, IBlockState blockState)
        {
            var index = Index(x, y, z);
            NBitsArray[index] = BlockPalette.GetId(blockState);
        }

        public IBlockState GetBlock(int x, int y, int z)
        {
            var index = Index(x, y, z);
            var id = NBitsArray[index];
            return BlockPalette.GetBlockState(id);
        }

        private static int Index(int x, int y, int z)
        {
            return (y & 15) << 8 | (z & 15) << 4 | x & 15;
        }
    }
}