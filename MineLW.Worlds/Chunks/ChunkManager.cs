using System;
using System.Collections.Generic;
using MineLW.API.Blocks.Palette;
using MineLW.API.Worlds.Chunks;
using MineLW.API.Worlds.Chunks.Generator;

namespace MineLW.Worlds.Chunks
{
    public class ChunkManager : IChunkManager
    {
        public IChunkGenerator Generator { get; set; }

        public int LoadedChunks => _loadedChunks.Count;

        private readonly Random _random = new Random();
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

        public bool CanGenerate(ChunkPosition position)
        {
            return Generator != null;
        }

        public IChunk GenerateChunk(ChunkPosition position)
        {
            if (IsLoaded(position))
                return GetChunk(position);
            if(Generator == null)
                throw new NotSupportedException("No chunk generator");
            
            var chunk = new Chunk(_globalPalette);
            Generator.Generate(position, chunk, _random);
            return _loadedChunks[position] = chunk;
        }

        public bool UnloadChunk(ChunkPosition position)
        {
            return _loadedChunks.Remove(position);
        }

        public IChunk GetChunk(ChunkPosition position)
        {
            return _loadedChunks[position];
        }
    }
}