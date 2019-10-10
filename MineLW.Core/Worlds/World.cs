using System;
using System.Collections.Generic;
using MineLW.API.Blocks;
using MineLW.API.Math;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;
using MineLW.Blocks;
using MineLW.Worlds.Chunks;

namespace MineLW.Worlds
{
    public class World : WorldContext, IWorld
    {
        public IChunkManager ChunkManager { get; }

        private readonly IWorldManager _worldManager;
        private readonly Dictionary<Identifier, WorldContext> _contexts = new Dictionary<Identifier, WorldContext>();

        public World(IWorldManager worldManager) : base(null)
        {
            _worldManager = worldManager;
            ChunkManager = new ChunkManager(this);
        }

        public IBlockState GetBlock(Vector3Int position)
        {
            var chunkPos = Chunk.BlockToChunkPosition(position);
            if(!ChunkManager.IsLoaded(chunkPos))
                return BlockState.Air;

            var chunk = ChunkManager.GetChunk(chunkPos);
            
            return BlockState.Air;
        }

        public void SetBlock(Vector3Int position, IBlockState blockState)
        {
            throw new NotSupportedException("Not implemented yet");
        }

        public IWorldContext CreateContext(Identifier name)
        {
            if (_contexts.ContainsKey(name))
                return _contexts[name];
            return _contexts[name] = new WorldContext(this);
        }
    }
}