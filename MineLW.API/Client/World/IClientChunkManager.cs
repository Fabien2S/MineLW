using System;
using MineLW.API.Blocks.Palette;
using MineLW.API.Worlds.Chunks;
using MineLW.API.Worlds.Chunks.Events;

namespace MineLW.API.Client.World
{
    public interface IClientChunkManager
    {
        event EventHandler<ChunkEventArgs> ChunkLoaded; 
        event EventHandler<ChunkEventArgs> ChunkUnloaded; 
        
        void SynchronizeChunks();
        IChunk RenderChunk(ChunkPosition position, IBlockPalette globalBlockPalette);
        bool IsLoaded(ChunkPosition position);
        void LoadChunk(ChunkPosition position);
        void UnloadChunk(ChunkPosition position);
    }
}