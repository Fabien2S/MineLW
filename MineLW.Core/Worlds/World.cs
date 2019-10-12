using System.Collections.Generic;
using MineLW.API;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Palette;
using MineLW.API.Math;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;
using MineLW.Blocks;
using MineLW.Worlds.Chunks;

namespace MineLW.Worlds
{
    public class World : WorldContext, IWorld
    {
        public IChunkManager ChunkManager { get; }

        private readonly Dictionary<Identifier, WorldContext> _contexts = new Dictionary<Identifier, WorldContext>();

        public World(IBlockPalette globalPalette) : base(null)
        {
            ChunkManager = new ChunkManager(globalPalette);
        }

        public IBlockState GetBlock(Vector3Int position)
        {
            var chunkPos = ChunkPosition.FromWorld(position);
            if(!ChunkManager.IsLoaded(chunkPos))
                return BlockState.Air;

            var chunk = ChunkManager.GetChunk(chunkPos);
            var index = Chunk.SectionIndex(position.Y);
            
            if(!chunk.HasSection(index))
                return BlockState.Air;

            var section = chunk[index];
            return section.BlockStorage.GetBlock(
                position.X % Minecraft.Units.Chunk.Size,
                position.Y % Minecraft.Units.Chunk.SectionHeight,
                position.Z % Minecraft.Units.Chunk.Size
            );
        }

        public void SetBlock(Vector3Int position, IBlockState blockState)
        {
            var chunkPos = ChunkPosition.FromWorld(position);
            if(!ChunkManager.IsLoaded(chunkPos))
                return;

            var chunk = ChunkManager.GetChunk(chunkPos);
            var index = Chunk.SectionIndex(position.Y);

            var section = chunk.CreateSection(index);
            section.BlockStorage.SetBlock(
                position.X % Minecraft.Units.Chunk.Size,
                position.Y % Minecraft.Units.Chunk.SectionHeight,
                position.Z % Minecraft.Units.Chunk.Size,
                blockState
            );
        }

        public IWorldContext CreateContext(Identifier name)
        {
            if (_contexts.ContainsKey(name))
                return _contexts[name];
            return _contexts[name] = new WorldContext(this);
        }
    }
}