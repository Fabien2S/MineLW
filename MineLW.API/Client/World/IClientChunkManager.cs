using MineLW.API.Worlds.Chunks;

namespace MineLW.API.Client.World
{
    public interface IClientChunkManager
    {
        void SynchronizeChunks();
        bool IsLoaded(ChunkPosition position);
        void LoadChunk(ChunkPosition position, IChunk chunk);
        void UnloadChunk(ChunkPosition position);
    }
}