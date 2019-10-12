using System.Collections.Generic;
using MineLW.API.Blocks.Palette;
using MineLW.API.Worlds.Chunks;
using MineLW.API.Worlds.Chunks.Generator;

namespace MineLW.Worlds.Chunks
{
    public class ChunkManager : IChunkManager
    {
        public IChunkGenerator Generator { get; set; }
        
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

        public IChunk GenerateChunk(ChunkPosition position)
        {
            if (IsLoaded(position))
                return GetChunk(position);
            
            var chunk = new Chunk(_globalPalette);
            Generator.Generate(chunk);
            return _loadedChunks[position] = chunk;
        }

        public IChunk CreateChunk(ChunkPosition position)
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