using System;
using MineLW.API.Entities;
using MineLW.API.Registries;
using MineLW.API.Utils;

namespace MineLW.Entities
{
    public class EntityRegistry : IEntityRegistry
    {
        private readonly Registry<Identifier, Type> _registry = new Registry<Identifier, Type>();

        public void Register<T>(Identifier name) where T : IEntity
        {
            _registry[name] = typeof(T);
        }

        public IEntity CreateEntity(Identifier name)
        {
            throw new NotImplementedException();
        }
    }
}