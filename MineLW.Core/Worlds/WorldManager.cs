using System;
using System.Collections.Generic;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Events;

namespace MineLW.Worlds
{
    public class WorldManager : IWorldManager
    {
        public Identifier DefaultWorld { get; set; } = Minecraft.CreateKey("overworld");

        public event EventHandler<WorldEventArgs> WorldCreated;

        private readonly Dictionary<Identifier, IWorld> _worlds = new Dictionary<Identifier, IWorld>();

        public IWorld CreateWorld(Identifier name)
        {
            if (_worlds.ContainsKey(name))
                return _worlds[name];

            var world = new World();
            WorldCreated?.Invoke(this, new WorldEventArgs(world));
            return _worlds[name] = world;
        }
    }
}