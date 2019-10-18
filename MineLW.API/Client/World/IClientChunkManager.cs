using MineLW.API.Worlds.Chunks;

namespace MineLW.API.Client.World
{
    public interface IClientChunkManager
    {
        void SynchronizeChunks();
        BakedChunk BakeChunk(ChunkPosition position);
        bool IsLoaded(ChunkPosition position);
        void LoadChunk(ChunkPosition position);
        void UnloadChunk(ChunkPosition position);
    }
}