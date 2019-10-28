using MineLW.API.Entities;

namespace MineLW.API.Client.World
{
    public interface IClientEntityManager
    {
        bool SpawnEntity(IEntity entity);
        int RemoveEntities(params IEntity[] entities);
    }
}