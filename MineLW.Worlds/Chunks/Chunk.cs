﻿using System;
using MineLW.API;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Palette;
using MineLW.API.Collections;
using MineLW.API.Worlds.Chunks;

namespace MineLW.Worlds.Chunks
{
    public class Chunk : IChunk
    {
        public NBitsArray HeightMap { get; } = NBitsArray.Create(
            9,
            Minecraft.Units.Chunk.Size * Minecraft.Units.Chunk.Size
        );

        private readonly IBlockPalette _globalPalette;
        private readonly ChunkSection[] _sections = new ChunkSection[Minecraft.Units.Chunk.SectionCount];

        public Chunk(IBlockPalette globalPalette)
        {
            _globalPalette = globalPalette;
        }

        public bool HasSection(int index)
        {
            return _sections[index] != null;
        }

        public IChunkSection CreateSection(int index)
        {
            if (HasSection(index))
                return _sections[index];
            return _sections[index] = new ChunkSection(_globalPalette);
        }

        public void RemoveSection(int index)
        {
            _sections[index] = null;
        }

        public bool HasBlock(int x, int y, int z)
        {
            var index = SectionIndex(y);
            if (!HasSection(index))
                return false;

            var section = _sections[index];
            var blockStorage = section.BlockStorage;
            return blockStorage.HasBlock(x, y % Minecraft.Units.Chunk.SectionHeight, z);
        }

        public IBlockState GetBlock(int x, int y, int z)
        {
            var index = SectionIndex(y);
            if (!HasSection(index))
                return null;

            var section = _sections[index];
            var blockStorage = section.BlockStorage;
            return blockStorage.GetBlock(x, y % Minecraft.Units.Chunk.SectionHeight, z);
        }

        public void SetBlock(int x, int y, int z, IBlockState blockState)
        {
            var index = SectionIndex(y);
            var section = CreateSection(index);
            var blockStorage = section.BlockStorage;
            blockStorage.SetBlock(
                x, y % Minecraft.Units.Chunk.SectionHeight, z, blockState
            );
        }

        public IChunkSection this[int index] => _sections[index];

        public static int SectionIndex(int y)
        {
            var index = (int) Math.Floor((float) y / Minecraft.Units.Chunk.SectionHeight);
            if (0 <= index && index < Minecraft.Units.Chunk.SectionCount)
                return index;
            throw new ArgumentOutOfRangeException(nameof(y), "Invalid Y (" + y + ')');
        }
    }
}