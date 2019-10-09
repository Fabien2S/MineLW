using System.Collections;
using System.Collections.Generic;
using MineLW.API.Entities;

namespace MineLW.Entities
{
    public class EntityManager : IEntityManager
    {
        private readonly List<IEntity> _entities = new List<IEntity>();

        public void Update(float deltaTime)
        {
            _entities.RemoveAll(e => !e.Valid);
            foreach (var entity in _entities)
                entity.Update(deltaTime);
        }

        public bool AddEntity(IEntity entity)
        {
            _entities.Add(entity);
            return true;
        }

        public bool RemoveEntity(IEntity entity)
        {
            return _entities.Remove(entity);
        }

        public IEnumerator<IEntity> GetEnumerator() => _entities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}