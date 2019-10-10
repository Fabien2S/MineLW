using MineLW.API.Blocks;

namespace MineLW.API.Worlds.Chunks
{
    public interface IChunkSection : IBlockStorage
    {
        IBlockStorage BlockStorage { get; }
    }
}