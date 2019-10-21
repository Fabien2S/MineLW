using System;
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

        /// <summary>
        /// Return if a chunk is loaded at the given position
        /// </summary>
        /// <param name="position">the chunk position</param>
        /// <returns>true if the chunk is loaded, false otherwise</returns>
        bool IsLoaded(ChunkPosition position);

        /// <summary>
        /// Return if a chunk can be generated at the given position in the current state of the <see cref="IChunkManager"/>
        /// </summary>
        /// <param name="position">The chunk position</param>
        /// <returns>true if the chunk can be generated, false otherwise</returns>
        /// <remarks>
        /// You should never cache the result of this method.
        /// The <see cref="IChunkManager"/> state can change at any time
        /// </remarks>
        bool CanGenerate(ChunkPosition position);

        /// <summary>
        /// Generates the chunk at the given position.
        /// If the chunk already exists, this one is returned and no generation occur
        /// </summary>
        /// <param name="position">The chunk position</param>
        /// <returns>The (generated) chunk</returns>
        /// <exception cref="NotSupportedException">If <see cref="Generator"/> is null</exception>
        IChunk GenerateChunk(ChunkPosition position);

        /// <summary>
        /// Unload the chunk at the given position
        /// </summary>
        /// <param name="position">The chunk position</param>
        void UnloadChunk(ChunkPosition position);
        
        /// <summary>
        /// Gets the chunk at the given position, or null if none
        /// </summary>
        /// <param name="position">The chunk</param>
        IChunk GetChunk(ChunkPosition position);
    }
}