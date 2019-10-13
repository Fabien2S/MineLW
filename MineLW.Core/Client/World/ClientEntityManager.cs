using System;
using MineLW.API.Client.World;
using MineLW.API.Entities;

namespace MineLW.Client.World
{
    public class ClientEntityManager : IClientEntityManager
    {
        public void SynchronizeEntities()
        {
        }

        public bool SpawnEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool RemoveEntities(params IEntity[] entities)
        {
            throw new NotImplementedException();
        }
    }
}