using System.Collections.Generic;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Context;

namespace MineLW.Worlds
{
    public class World : WorldContext, IWorld
    {
        private readonly Dictionary<Identifier, WorldContext> _contexts = new Dictionary<Identifier, WorldContext>();

        public World() : base(null)
        {
        }

        public IWorldContext CreateContext(Identifier name)
        {
            if (_contexts.ContainsKey(name))
                return _contexts[name];
            return _contexts[name] = new WorldContext(this);
        }
    }
}