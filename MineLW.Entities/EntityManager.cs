﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MineLW.API.Client;
using MineLW.API.Entities;
using MineLW.API.Entities.Events;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Math;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;
using MineLW.Entities.Living.Player;

namespace MineLW.Entities
{
    public class EntityManager : IEntityManager
    {
        public event EventHandler<EntityEventArgs> EntitySpawned;
        public event EventHandler<EntityEventArgs> EntityRemoved;

        private readonly IWorldContext _worldContext;
        private readonly IUidGenerator _uidGenerator;
        private readonly HashSet<IEntity> _entities = new HashSet<IEntity>();

        public EntityManager(IWorldContext worldContext, IUidGenerator uidGenerator)
        {
            _uidGenerator = uidGenerator;
            _worldContext = worldContext;
        }

        public void Update(float deltaTime)
        {
            _entities.RemoveWhere(e => !e.Valid);
            foreach (var e in _entities)
                e.Update(deltaTime);
        }

        private void OnEntityWorldChanged(object sender, EntityWorldChangedEventArgs e)
        {
            var entity = (IEntity) sender;
            RemoveEntity(entity);

            var destination = e.To;
            var entityManager = (EntityManager) destination.EntityManager;
            entityManager.AddEntity(entity);
        }

        private void OnEntityRemoved(object sender, EventArgs e)
        {
            RemoveEntity((IEntity) sender);
        }

        public IEnumerable<IEntity> GetEntities(ChunkPosition position)
        {
            return _entities.Where(e => ChunkPosition.FromWorld(e.Position) == position);
        }

        public IEntity SpawnEntity(Identifier name, Vector3 position, Rotation rotation)
        {
            // TODO resolve entity type from name through game adapter

            var uid = _uidGenerator.GenerateUid();
            IEntity entity = new EntityPlayer(uid, Guid.NewGuid())
            {
                WorldContext = _worldContext,
                Position = position,
                Rotation = rotation
            };
            AddEntity(entity);
            return entity;
        }

        public IEntityPlayer SpawnPlayer(IClient client, Vector3 position, Rotation rotation)
        {
            var uid = _uidGenerator.GenerateUid();
            var player = new EntityPlayer(uid, client)
            {
                WorldContext = _worldContext,
                Position = position,
                Rotation = rotation
            };
            AddEntity(player);
            return player;
        }

        private void AddEntity(IEntity entity)
        {
            if (!_entities.Add(entity))
                return;
            
            entity.WorldChanged += OnEntityWorldChanged;
            entity.Removed += OnEntityRemoved;

            EntitySpawned?.Invoke(this, new EntityEventArgs(entity));
        }

        private void RemoveEntity(IEntity entity)
        {
            if (!_entities.Remove(entity))
                return;

            entity.WorldChanged -= OnEntityWorldChanged;
            entity.Removed -= OnEntityRemoved;
            
            EntityRemoved?.Invoke(this, new EntityEventArgs(entity));
        }

        public IEnumerator<IEntity> GetEnumerator() => _entities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}