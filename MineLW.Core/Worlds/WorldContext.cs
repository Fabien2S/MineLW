using System.Collections.Generic;
using MineLW.API.Entities;
using MineLW.API.Worlds;
using MineLW.Entities;

namespace MineLW.Worlds
{
    public class WorldContext : IWorldContext
    {
        public IWorld World => _world ?? this as World;
        public IEntityManager EntityManager { get; } = new EntityManager();

        private readonly World _world;
        private readonly Dictionary<WorldOption, dynamic> _options = new Dictionary<WorldOption, dynamic>();

        public WorldContext(World world)
        {
            _world = world;
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