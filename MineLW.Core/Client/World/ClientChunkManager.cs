using System.Collections.Generic;
using MineLW.API.Client.World;
using MineLW.API.Worlds.Chunks;
using NLog;

namespace MineLW.Client.World
{
    public class ClientChunkManager : IClientChunkManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private readonly ISet<ChunkPosition> _loadedChunks = new HashSet<ChunkPosition>();
        
        public bool IsLoaded(ChunkPosition position)
        {
            return _loadedChunks.Contains(position);
        }

        public void LoadChunk(ChunkPosition position, IChunk chunk)
        {
            if(!_loadedChunks.Add(position))
            {
                Logger.Warn("Trying to load an already loaded chunk at {0}", position);
                return;
            }
            
            
        }

        public void UnloadChunk(ChunkPosition position)
        {
            if(!_loadedChunks.Remove(position))
            {
                Logger.Warn("Trying to unload an unloaded chunk at {0}", position);
                return;
            }
            
            
        }
    }
}