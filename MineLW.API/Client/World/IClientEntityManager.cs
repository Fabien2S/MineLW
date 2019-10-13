using MineLW.API.Entities;

namespace MineLW.API.Client.World
{
    public interface IClientEntityManager
    {
        void SynchronizeEntities();
        bool SpawnEntity(IEntity entity);
        bool RemoveEntities(params IEntity[] entities);
    }
}