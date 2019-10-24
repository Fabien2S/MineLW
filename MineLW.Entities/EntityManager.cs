using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using MineLW.API.Entities;
using MineLW.API.Entities.Events;
using MineLW.API.Math;
using MineLW.API.Utils;

namespace MineLW.Entities
{
    public class EntityManager : IEntityManager
    {
        private readonly HashSet<IEntity> _entities = new HashSet<IEntity>();

        public void Update(float deltaTime)
        {
            _entities.RemoveWhere(e => !e.Valid);
            foreach (var e in _entities)
                e.Update(deltaTime);
        }

        private void OnEntityWorldChanged(object sender, EntityWorldChangedEventArgs e)
        {
            var entity = e.Entity;
            RemoveEntity(entity);

            var destination = e.To;
            var entityManager = destination.EntityManager;
            if(entityManager is EntityManager internalEntityManager)
                internalEntityManager.AddEntity(entity);
        }

        private void OnEntityRemoved(object sender, EntityEventArgs e)
        {
            var entity = e.Entity;
            RemoveEntity(entity);
        }

        public IEntity SpawnEntity(Identifier name, Vector3 position, Rotation rotation)
        {
            // TODO resolve entity type from name through game adapter
            throw new NotImplementedException();
            
            IEntity entity = null;
            AddEntity(entity);
        }

        private void AddEntity(IEntity entity)
        {
            if (!_entities.Add(entity))
                return;
            
            entity.WorldChanged += OnEntityWorldChanged;
            entity.Removed += OnEntityRemoved;
        }

        private void RemoveEntity(IEntity entity)
        {
            if (!_entities.Remove(entity))
                return;
            
            entity.WorldChanged -= OnEntityWorldChanged;
            entity.Removed -= OnEntityRemoved;
        }

        public IEnumerator<IEntity> GetEnumerator() => _entities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}