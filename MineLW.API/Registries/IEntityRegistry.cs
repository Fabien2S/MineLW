using MineLW.API.Entities;
using MineLW.API.Utils;

namespace MineLW.API.Registries
{
    public interface IEntityRegistry
    {
        void Register<T>(Identifier name) where T : IEntity;
        IEntity CreateEntity(Identifier name);
    }
}