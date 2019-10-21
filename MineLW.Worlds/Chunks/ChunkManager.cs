using System;
using System.Collections.Generic;
using MineLW.API.Blocks.Palette;
using MineLW.API.Worlds.Chunks;
using MineLW.API.Worlds.Chunks.Generator;
using NLog;

namespace MineLW.Worlds.Chunks
{
    public class ChunkManager : IChunkManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
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
            Logger.Debug("Generating chunk at {0}", position);
            Generator.Generate(chunk);
            return _loadedChunks[position] = chunk;
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