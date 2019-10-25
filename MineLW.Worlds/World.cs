using System.Collections.Generic;
using MineLW.API.Blocks.Palette;
using MineLW.API.Utils;
using MineLW.API.Worlds;

namespace MineLW.Worlds
{
    public class World : WorldContext, IWorld
    {
        private readonly IUidGenerator _uidGenerator;
        private readonly IBlockPalette _globalPalette;
        private readonly Dictionary<Identifier, WorldContext> _contexts = new Dictionary<Identifier, WorldContext>();

        public World(IUidGenerator uidGenerator, IBlockPalette globalPalette) : base(null, uidGenerator, globalPalette)
        {
            _uidGenerator = uidGenerator;
            _globalPalette = globalPalette;
        }

        public IWorldContext CreateContext(Identifier name)
        {
            if (_contexts.ContainsKey(name))
                return _contexts[name];
            return _contexts[name] = new WorldContext(this, _uidGenerator, _globalPalette);
        }
    }
}