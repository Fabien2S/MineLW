using MineLW.API.Worlds.Chunks.Generator;

namespace MineLW.API.Worlds.Chunks
{
    public interface IChunkManager
    {
        /// <summary>
        /// The generator used to generate chunks
        /// <remarks>If the generator is set to null, no chunk will be generated</remarks>
        /// </summary>
        IChunkGenerator Generator { get; set; }

        bool IsLoaded(ChunkPosition position);
        IChunk CreateChunk(ChunkPosition position);
        IChunk GenerateChunk(ChunkPosition position);
        void UnloadChunk(ChunkPosition position);
        IChunk GetChunk(ChunkPosition position);
    }
}