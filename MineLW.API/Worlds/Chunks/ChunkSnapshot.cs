using MineLW.API.Blocks;
using MineLW.API.Utils;

namespace MineLW.API.Worlds.Chunks
{
    public class ChunkSnapshot
    {
        public readonly NBitsArray HeightMap;
        public readonly int SectionMask;
        public readonly IBlockStorage[] BlockStorage;

        public ChunkSnapshot(NBitsArray heightMap, int sectionMask, IBlockStorage[] blockStorage)
        {
            HeightMap = heightMap;
            SectionMask = sectionMask;
            BlockStorage = blockStorage;
        }
    }
}