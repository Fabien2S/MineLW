using MineLW.API.Blocks;
using MineLW.API.Blocks.Palette;
using MineLW.API.Worlds.Chunks;
using MineLW.Blocks;

namespace MineLW.Worlds.Chunks
{
    public class ChunkSection : IChunkSection
    {
        public const int BlockCount = Chunk.Size * Chunk.SectionCount * Chunk.Size;
        
        public IBlockStorage BlockStorage { get; }

        public ChunkSection(IBlockPalette globalPalette)
        {
            BlockStorage = new BlockStorage(globalPalette);
        }
    }
}