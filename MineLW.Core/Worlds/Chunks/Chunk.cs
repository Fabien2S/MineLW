using System;
using MineLW.API;
using MineLW.API.Blocks.Palette;
using MineLW.API.Worlds.Chunks;

namespace MineLW.Worlds.Chunks
{
    public class Chunk : IChunk
    {
        private readonly IBlockPalette _globalPalette;
        private readonly ChunkSection[] _sections = new ChunkSection[Minecraft.Units.Chunk.SectionCount];

        public Chunk(IBlockPalette globalPalette)
        {
            _globalPalette = globalPalette;
        }

        public bool HasSection(int index)
        {
            return _sections[index] != null;
        }

        public IChunkSection CreateSection(int index)
        {
            if (HasSection(index))
                return _sections[index];
            return _sections[index] = new ChunkSection(_globalPalette);
        }

        public void RemoveSection(int index)
        {
            _sections[index] = null;
        }

        public IChunkSection this[int index] => _sections[index];

        public static int SectionIndex(int y)
        {
            var index = (int) Math.Floor((float) y / Minecraft.Units.Chunk.SectionHeight);
            if (0 <= index && index < Minecraft.Units.Chunk.SectionCount)
                return index;
            throw new ArgumentOutOfRangeException(nameof(y), "Invalid Y (" + y + ')');
        }
    }
}