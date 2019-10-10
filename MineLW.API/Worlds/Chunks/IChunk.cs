namespace MineLW.API.Worlds.Chunks
{
    public interface IChunk
    {
        bool HasSection(int index);
        IChunkSection CreateSection(int index);
        void RemoveSection(int index);

        IChunkSection this[int index] { get; }
    }
}