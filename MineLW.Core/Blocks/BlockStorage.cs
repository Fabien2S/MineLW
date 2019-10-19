using System;
using DotNetty.Buffers;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Palette;
using MineLW.API.Utils;
using MineLW.Blocks.Palette;
using MineLW.Networking.IO;

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
        /// Resize the storage to match the bitsPerBlock
        /// </summary>
        /// <param name="bitsPerBlock">The number of bits used to store a block state</param>
        private void Resize(byte bitsPerBlock)
        {
            var previousDataBits = NBitsArray;
            var previousBlockPalette = BlockPalette;

            if (!UpdatePalette(bitsPerBlock))
                return;
            
            for (var i = 0; i < previousDataBits.Capacity; i++)
            {
                var blockStateId = previousDataBits[i];
                if (blockStateId == 0)
                    continue;

                var blockState = previousBlockPalette.GetBlockState(blockStateId);
                NBitsArray[i] = BlockPalette.GetId(blockState);
            }
        }

        /// <summary>
        /// Update the internal palette to match the bitsPerBlock
        /// </summary>
        /// <remarks>This should never be called directly, please use <see cref="Resize"/></remarks>
        /// <param name="bitsPerBlock">The number of bits used to store a block state</param>
        /// <returns>true if the internal array must be updated, false otherwise</returns>
        private bool UpdatePalette(byte bitsPerBlock)
        {
            bitsPerBlock = Math.Clamp(bitsPerBlock, MinBitsPerBlock, _globalPalette.BitsPerBlock);

            if (BlockPalette.BitsPerBlock == bitsPerBlock)
                return false;

            var flag = true;
            if (bitsPerBlock <= 8)
            {
                if (BlockPalette is LinearBlockPalette blockPalette)
                {
                    blockPalette.Resize(bitsPerBlock);
                    flag = false;
                }
                else
                    BlockPalette = new LinearBlockPalette(_globalPalette, bitsPerBlock);
            }
            else
                BlockPalette = _globalPalette;

            NBitsArray = NBitsArray.Create(bitsPerBlock, 4096);
            return flag;
        }

        public bool HasBlock(int x, int y, int z)
        {
            var index = Index(x, y, z);
            return NBitsArray[index] != 0;
        }

        public void SetBlock(int x, int y, int z, IBlockState blockState)
        {
            var id = BlockPalette.GetId(blockState);
            if (id == -1) // block not registered
            {
                Resize((byte) (BlockPalette.BitsPerBlock + 1));
                id = BlockPalette.GetId(blockState);
                if (id == -1)
                    throw new InvalidOperationException("Invalid block state palette id");
            }

            var index = Index(x, y, z);
            NBitsArray[index] = id;
        }

        public IBlockState GetBlock(int x, int y, int z)
        {
            var index = Index(x, y, z);
            var id = NBitsArray[index];
            return BlockPalette.GetBlockState(id);
        }

        public void Serialize(IByteBuffer buffer)
        {
            var backing = NBitsArray.Backing;
            buffer.WriteVarInt32(backing.Length);
            foreach (var l in backing)
                buffer.WriteLong(l);
        }

        private static int Index(int x, int y, int z)
        {
            return (y & 15) << 8 | (z & 15) << 4 | x & 15;
        }
    }
}