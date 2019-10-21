using System.Collections.Generic;
using MineLW.API;
using MineLW.API.Client;
using MineLW.API.Client.World;
using MineLW.API.Worlds.Chunks;
using NLog;

namespace MineLW.Clients.World
{
    public class ClientChunkManager : IClientChunkManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IClient _client;
        private readonly ISet<ChunkPosition> _loadedChunks = new HashSet<ChunkPosition>();

        public ClientChunkManager(IClient client)
        {
            _client = client;
        }

        public void SynchronizeChunks()
        {
            var clientWorld = _client.World;
            var renderDistance = clientWorld.RenderDistance;
            var clientPosition = clientWorld.ChunkPosition;

            ISet<ChunkPosition> chunkToUnload = new HashSet<ChunkPosition>(_loadedChunks);
            ISet<ChunkPosition> chunkToLoad = new HashSet<ChunkPosition>();

            for (var x = clientPosition.X - renderDistance; x <= clientPosition.X + renderDistance; x++)
            for (var z = clientPosition.Z - renderDistance; z <= clientPosition.Z + renderDistance; z++)
            {
                var chunkPosition = new ChunkPosition(x, z);
                if (chunkToUnload.Contains(chunkPosition))
                    chunkToUnload.Remove(chunkPosition);
                else
                    chunkToLoad.Add(chunkPosition);
            }

            Logger.Debug("Loading {0} chunk(s)", chunkToLoad.Count);
            foreach (var position in chunkToLoad)
                LoadChunk(position);

            Logger.Debug("Unloading {0} chunk(s)", chunkToUnload.Count);
            foreach (var position in chunkToUnload)
                UnloadChunk(position);
        }

        public BakedChunk BakeChunk(ChunkPosition position)
        {
            var bakedChunk = new BakedChunk();

            var worldContexts = _client.World.WorldContexts;
            foreach (var worldContext in worldContexts)
            {
                var chunkManager = worldContext.ChunkManager;
                var chunk = chunkManager.GetChunk(position);

                for (var x = 0; x < Minecraft.Units.Chunk.Size; x++)
                for (var z = 0; z < Minecraft.Units.Chunk.Size; z++)
                for (var y = 0; y < Minecraft.Units.Chunk.Height; y++)
                {
                    if(bakedChunk.HasBlock(x, y, z))
                        continue;
                    if (!chunk.HasBlock(x, y, z))
                        continue;
                    
                    var blockState = chunk.GetBlock(x, y, z);
                    bakedChunk.SetBlock(x, y, z, blockState);
                }
            }

            return bakedChunk;
        }

        public bool IsLoaded(ChunkPosition position)
        {
            return _loadedChunks.Contains(position);
        }

        public void LoadChunk(ChunkPosition position)
        {
            if (!_loadedChunks.Add(position))
            {
                Logger.Warn("Trying to load an already loaded chunk at {0}", position);
                return;
            }

            var bakedChunk = BakeChunk(position);
            _client.Connection.LoadChunk(position, bakedChunk);
        }

        public void UnloadChunk(ChunkPosition position)
        {
            if (!_loadedChunks.Remove(position))
            {
                Logger.Warn("Trying to unload an unloaded chunk at {0}", position);
                return;
            }

            _client.Connection.UnloadChunk(position);
        }
    }
}