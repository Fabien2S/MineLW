using System;
using MineLW.API.Math;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;

namespace MineLW.Worlds.Chunks
{
    public class ChunkManager : IChunkManager
    {
        private readonly IWorld _world;

        public ChunkManager(IWorld world)
        {
            _world = world;
        }

        public bool IsLoaded(Vector2Int position)
        {
            throw new NotImplementedException();
        }

        public IChunk LoadChunk(Vector2Int position)
        {
            throw new NotImplementedException();
        }

        public IChunk UnloadChunk(Vector2Int position)
        {
            throw new NotImplementedException();
        }

        public IChunk GetChunk(Vector2Int position)
        {
            throw new NotImplementedException();
        }
    }
}