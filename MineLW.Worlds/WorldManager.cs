using System;
using System.Collections.Generic;
using MineLW.API;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Palette;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Events;
using MineLW.Blocks.Palette;

namespace MineLW.Worlds
{
    public class WorldManager : IWorldManager
    {
        public Identifier DefaultWorld { get; set; } = Minecraft.CreateIdentifier("overworld");

        public event EventHandler<WorldEventArgs> WorldCreated;

        private readonly IBlockPalette _globalPalette;
        private readonly Dictionary<Identifier, IWorld> _worlds = new Dictionary<Identifier, IWorld>();

        public WorldManager(IBlockManager blockManager)
        {
            _globalPalette = new GlobalBlockPalette(blockManager);
        }
        
        public IWorld CreateWorld(Identifier name)
        {
            if (_worlds.ContainsKey(name))
                return _worlds[name];

            var world = new World(_globalPalette);
            WorldCreated?.Invoke(this, new WorldEventArgs(world));
            return _worlds[name] = world;
        }
    }
}