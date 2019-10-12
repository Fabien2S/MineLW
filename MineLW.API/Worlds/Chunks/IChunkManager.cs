namespace MineLW.API.Worlds.Chunks
{
    public interface IChunkManager
    {
        bool IsLoaded(ChunkPosition position);
        IChunk LoadChunk(ChunkPosition position);
        void UnloadChunk(ChunkPosition position);
        IChunk GetChunk(ChunkPosition position);
    }
}