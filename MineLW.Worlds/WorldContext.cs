using System.Collections.Generic;
using MineLW.API;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Palette;
using MineLW.API.Entities;
using MineLW.API.Math;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;
using MineLW.Entities;
using MineLW.Worlds.Chunks;

namespace MineLW.Worlds
{
    public class WorldContext : IWorldContext
    {
        public IWorld World => _world ?? this as World;

        public IChunkManager ChunkManager { get; }
        public IEntityManager EntityManager { get; } = new EntityManager();

        private readonly World _world;
        private readonly Dictionary<WorldOption, dynamic> _options = new Dictionary<WorldOption, dynamic>();

        public WorldContext(World world, IBlockPalette globalPalette)
        {
            _world = world;
            ChunkManager = new ChunkManager(globalPalette);
        }

        public IBlockState GetBlock(Vector3Int position)
        {
            var chunkPos = ChunkPosition.FromWorld(position);
            if (!ChunkManager.IsLoaded(chunkPos))
                return null;

            var chunk = ChunkManager.GetChunk(chunkPos);
            var index = Chunk.SectionIndex(position.Y);

            if (!chunk.HasSection(index))
                return null;

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
            if (!ChunkManager.IsLoaded(chunkPos))
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

        public T GetOptionRaw<T>(WorldOption<T> option)
        {
            return _options[option];
        }

        public T GetOption<T>(WorldOption<T> option)
        {
            if (_options.ContainsKey(option))
                return _options[option];
            return _world != null ? _world.GetOption(option) : option.DefaultValue;
        }

        public void SetOption<T>(WorldOption<T> option, T value)
        {
            _options[option] = value;
        }
    }
}