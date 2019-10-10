using MineLW.API.Math;

namespace MineLW.API.Worlds.Chunks
{
    public interface IChunkManager
    {
        bool IsLoaded(Vector2Int position);
        IChunk LoadChunk(Vector2Int position);
        IChunk UnloadChunk(Vector2Int position);
        IChunk GetChunk(Vector2Int position);
    }
}