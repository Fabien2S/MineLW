namespace MineLW.API.Worlds.Chunks.Generator
{
    public interface IChunkDecorator
    {
        void Decorate(IChunk snapshot);
    }
}