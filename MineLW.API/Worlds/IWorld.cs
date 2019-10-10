using MineLW.API.Blocks;
using MineLW.API.Math;
using MineLW.API.Utils;
using MineLW.API.Worlds.Chunks;

namespace MineLW.API.Worlds
{
    public interface IWorld : IWorldContext
    {
        IChunkManager ChunkManager { get; }
        
        IBlockState GetBlock(Vector3Int position);
        void SetBlock(Vector3Int position, IBlockState blockState);

        IWorldContext CreateContext(Identifier name);
    }
}