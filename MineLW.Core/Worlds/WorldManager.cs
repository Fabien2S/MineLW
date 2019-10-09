using System.Collections.Generic;
using MineLW.API.Utils;
using MineLW.API.Worlds;

namespace MineLW.Worlds
{
    public class WorldManager : IWorldManager
    {
        public Identifier DefaultWorld { get; set; } = Minecraft.CreateKey("overworld");

        private readonly Dictionary<Identifier, IWorld> _worlds = new Dictionary<Identifier, IWorld>();

        public IWorld CreateWorld(Identifier name)
        {
            if (_worlds.ContainsKey(name))
                return _worlds[name];

            var world = new World();
            return _worlds[name] = world;
        }
    }
}