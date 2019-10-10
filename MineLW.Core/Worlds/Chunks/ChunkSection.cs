using MineLW.Adapters;
using MineLW.API.Blocks;
using MineLW.API.Worlds.Chunks;

namespace MineLW.Worlds.Chunks
{
    public class ChunkSection : IChunkSection
    {
        public IBlockStorage BlockStorage { get; }

        public ChunkSection()
        {
            var serverAdapter = GameAdapter.ServerAdapter;
            BlockStorage = serverAdapter.CreateBlockStorage();
        }
    }
}