using MineLW.API.Blocks;

namespace MineLW.API.Worlds.Chunks
{
    public interface IChunk : IBlockStorage
    {
        bool HasSection(int index);
        IChunkSection CreateSection(int index);
        void RemoveSection(int index);

        IChunkSection this[int index] { get; }
    }
}