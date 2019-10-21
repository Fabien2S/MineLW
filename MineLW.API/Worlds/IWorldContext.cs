using MineLW.API.Blocks;
using MineLW.API.Entities;
using MineLW.API.Math;
using MineLW.API.Worlds.Chunks;

namespace MineLW.API.Worlds
{
    public interface IWorldContext
    {
        IWorld World { get; }
        IChunkManager ChunkManager { get; }
        IEntityManager EntityManager { get; }

        /// <summary>
        /// Gets the block at the current position, or null
        /// </summary>
        /// <param name="position">the position when the block is located</param>
        /// <returns>the block, or null if empty</returns>
        IBlockState GetBlock(Vector3Int position);

        void SetBlock(Vector3Int position, IBlockState blockState);

        T GetOptionRaw<T>(WorldOption<T> option);
        T GetOption<T>(WorldOption<T> option);
        void SetOption<T>(WorldOption<T> option, T value);
    }
}