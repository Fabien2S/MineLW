using System.Collections.Generic;
using MineLW.API.Math;
using MineLW.API.Worlds.Chunks;

namespace MineLW.Worlds.Chunks
{
    public class ChunkManager : IChunkManager
    {
        private readonly Dictionary<Vector2Int, IChunk> _loadedChunks = new Dictionary<Vector2Int, IChunk>();

        public bool IsLoaded(Vector2Int position)
        {
            return _loadedChunks.ContainsKey(position);
        }

        public IChunk LoadChunk(Vector2Int position)
        {
            if (IsLoaded(position))
                return GetChunk(position);
            return _loadedChunks[position] = new Chunk();
        }

        public void UnloadChunk(Vector2Int position)
        {
            _loadedChunks.Remove(position);
        }

        public IChunk GetChunk(Vector2Int position)
        {
            return _loadedChunks[position];
        }
    }
}