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
        private readonly IBlockPalette _globalPalette;
        private readonly Dictionary<Identifier, WorldContext> _contexts = new Dictionary<Identifier, WorldContext>();

        public World(IBlockPalette globalPalette) : base(null, globalPalette)
        {
            _globalPalette = globalPalette;
        }

        public IWorldContext CreateContext(Identifier name)
        {
            if (_contexts.ContainsKey(name))
                return _contexts[name];
            return _contexts[name] = new WorldContext(this, _globalPalette);
        }
    }
}