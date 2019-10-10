using System;
using MineLW.API.Math;
using MineLW.API.Worlds.Chunks;

namespace MineLW.Worlds.Chunks
{
    public class Chunk : IChunk
    {
        public const int Size = 16;
        public const int Height = 256;

        public const int SectionHeight = 16;
        public const int SectionCount = Height / SectionHeight;

        private readonly ChunkSection[] _sections = new ChunkSection[SectionCount];
        private int _sectionMask = 0;

        private int SectionIndex(int y)
        {
            var index = (int) Math.Floor((float) y / SectionHeight);
            if (0 <= index && index < SectionCount)
                return index;
            throw new ArgumentOutOfRangeException(nameof(y), "Invalid Y (" + y + ')');
        }

        public bool HasSection(int index)
        {
            return (_sectionMask & (1 << index)) != 0;
        }

        public IChunkSection CreateSection(int index)
        {
            if (HasSection(index))
                return _sections[index];

            _sectionMask |= 1 << index;
            return _sections[index] = new ChunkSection();
        }

        public void RemoveSection(int index)
        {
            if (!HasSection(index))
                return;

            _sectionMask &= ~(1 << index);
            _sections[index] = null;
        }

        public IChunkSection this[int index] => _sections[index];

        public static Vector2Int BlockToChunkPosition(Vector3Int blockPosition)
        {
            return new Vector2Int(
                blockPosition.X / Size,
                blockPosition.Z / Size
            );
        }
    }
}