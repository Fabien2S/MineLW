using System.Collections.Generic;
using MineLW.API.Blocks.Palette;
using MineLW.API.Worlds.Chunks;

namespace MineLW.Worlds.Chunks
{
    public class ChunkManager : IChunkManager
    {
        private readonly IBlockPalette _globalPalette;
        private readonly Dictionary<ChunkPosition, IChunk> _loadedChunks = new Dictionary<ChunkPosition, IChunk>();

        public ChunkManager(IBlockPalette globalPalette)
        {
            _globalPalette = globalPalette;
        }

        public bool IsLoaded(ChunkPosition position)
        {
            return _loadedChunks.ContainsKey(position);
        }

        public IChunk LoadChunk(ChunkPosition position)
        {
            if (IsLoaded(position))
                return GetChunk(position);
            return _loadedChunks[position] = new Chunk(_globalPalette);
        }

        public void UnloadChunk(ChunkPosition position)
        {
            _loadedChunks.Remove(position);
        }

        public IChunk GetChunk(ChunkPosition position)
        {
            return _loadedChunks[position];
        }
    }
}