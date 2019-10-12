namespace MineLW.API.Worlds.Chunks.Generator
{
    public interface IChunkGenerator
    {
        void Generate(IChunk chunk);
    }
}