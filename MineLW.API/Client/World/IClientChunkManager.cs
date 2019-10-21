using MineLW.API.Blocks.Palette;
using MineLW.API.Worlds.Chunks;

namespace MineLW.API.Client.World
{
    public interface IClientChunkManager
    {
        void SynchronizeChunks();
        IChunk RenderChunk(ChunkPosition position, IBlockPalette globalBlockPalette);
        bool IsLoaded(ChunkPosition position);
        void LoadChunk(ChunkPosition position);
        void UnloadChunk(ChunkPosition position);
    }
}