using MineLW.API.Blocks.Palette;
using MineLW.API.IO;

namespace MineLW.API.Blocks
{
    public interface IBlockStorage : IByteBufferSerializable
    {
        IBlockPalette BlockPalette { get; }
        ushort BlockCount { get; }

        /// <summary>
        /// Return if a block is present at the given position
        /// </summary>
        /// <param name="x">The x axis</param>
        /// <param name="y">The y axis</param>
        /// <param name="z">The z axis</param>
        /// <returns>true is there is a block, false otherwise</returns>
        bool HasBlock(int x, int y, int z);
        
        /// <summary>
        /// Gets the block at the given position, or null
        /// </summary>
        /// <param name="x">The x axis</param>
        /// <param name="y">The y axis</param>
        /// <param name="z">The z axis</param>
        /// <returns>the block, or null</returns>
        IBlockState GetBlock(int x, int y, int z);


        /// <summary>
        /// Sets the block at the given position
        /// </summary>
        /// <param name="x">The x axis</param>
        /// <param name="y">The y axis</param>
        /// <param name="z">The z axis</param>
        /// <param name="blockState">The block to set, or null</param>
        void SetBlock(int x, int y, int z, IBlockState blockState);
    }
}