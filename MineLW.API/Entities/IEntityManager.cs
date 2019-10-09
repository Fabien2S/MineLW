using System.Collections.Generic;
using MineLW.API.Utils;

namespace MineLW.API.Entities
{
    public interface IEntityManager : IUpdatable, IEnumerable<IEntity>
    {
        bool AddEntity(IEntity entity);
        bool RemoveEntity(IEntity entity);
    }
}