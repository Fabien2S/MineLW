﻿using System.Collections.Generic;
using System.Linq;
using MineLW.API.Client;
using MineLW.API.Client.World;
using MineLW.API.Entities;
using MineLW.API.Entities.Events;
using MineLW.API.Worlds.Chunks;
using MineLW.API.Worlds.Chunks.Events;
using MineLW.API.Worlds.Events;
using NLog;

namespace MineLW.Clients.World
{
    public class ClientEntityManager : IClientEntityManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IClient _client;
        private readonly IClientWorld _world;
        private readonly ISet<IEntity> _loadedEntities = new HashSet<IEntity>();

        public ClientEntityManager(IClient client, IClientWorld world)
        {
            _client = client;
            _world = world;

            world.WorldContextRegistered += OnWorldContextRegistered;
            world.WorldContextUnregistered += OnWorldContextUnregistered;

            var chunkManager = world.ChunkManager;
            chunkManager.ChunkLoaded += OnChunkLoaded;
            chunkManager.ChunkUnloaded += OnChunkUnloaded;
        }
        
        private void OnWorldContextRegistered(object sender, WorldContextEventArgs e)
        {
            var chunkManager = _world.ChunkManager;
            
            var worldContext = e.WorldContext;
            var entityManager = worldContext.EntityManager;
            
            foreach (var entity in entityManager)
            {
                var chunkPosition = ChunkPosition.FromWorld(entity.Position);
                if (chunkManager.IsLoaded(chunkPosition))
                    SpawnEntity(entity);
            }

            entityManager.EntitySpawned += OnEntitySpawned;
            entityManager.EntityRemoved += OnEntityRemoved;
        }

        private void OnWorldContextUnregistered(object sender, WorldContextEventArgs e)
        {
            var worldContext = e.WorldContext;
            var entityManager = worldContext.EntityManager;

            var loadedEntities = entityManager.Where(ent => _loadedEntities.Contains(ent));
            RemoveEntities(loadedEntities);

            entityManager.EntitySpawned -= OnEntitySpawned;
            entityManager.EntityRemoved -= OnEntityRemoved;
        }

        private void OnChunkLoaded(object sender, ChunkEventArgs e)
        {
            var worldContexts = _world.WorldContexts;
            foreach (var worldContext in worldContexts)
            {
                var entityManager = worldContext.EntityManager;
                var entities = entityManager.GetEntities(e.Position);
                foreach (var entity in entities)
                    SpawnEntity(entity);
            }
        }

        private void OnChunkUnloaded(object sender, ChunkEventArgs e)
        {
            var toRemove = new HashSet<IEntity>();
            
            var unloadedChunk = e.Position;
            var worldContexts = _world.WorldContexts;
            foreach (var worldContext in worldContexts)
            {
                var entityManager = worldContext.EntityManager;
                foreach (var entity in entityManager)
                {
                    var chunkPosition = ChunkPosition.FromWorld(entity.Position);
                    if(unloadedChunk != chunkPosition)
                        continue;

                    toRemove.Add(entity);
                }
            }

            RemoveEntities(toRemove);
        }

        private void OnEntitySpawned(object sender, EntityEventArgs e)
        {
            var entity = e.Entity;
            var position = ChunkPosition.FromWorld(entity.Position);
            var chunkManager = _world.ChunkManager;
            if (chunkManager.IsLoaded(position))
                SpawnEntity(entity);
        }

        private void OnEntityRemoved(object sender, EntityEventArgs e)
        {
            var entity = e.Entity;
            if(_loadedEntities.Contains(entity))
                RemoveEntities(entity);
        }

        public bool SpawnEntity(IEntity entity)
        {
            var player = _client.Player;
            if (player.Id == entity.Id)
                return false;

            var chunkManager = _world.ChunkManager;
            var chunkPosition = ChunkPosition.FromWorld(entity.Position);
            if (!chunkManager.IsLoaded(chunkPosition))
            {
                Logger.Warn("Trying to spawn {0} in an unloaded chunk", entity);
                return false;
            }

            if (!_loadedEntities.Add(entity))
                return false;

            Logger.Info("Spawning entity #{0} on {1}", entity.Id, _client);
            var connection = _client.Connection;
            connection.SpawnEntity(entity);
            return true;
        }

        public int RemoveEntities(params IEntity[] entities)
        {
            return RemoveEntities((IEnumerable<IEntity>) entities);
        }

        private int RemoveEntities(IEnumerable<IEntity> entities)
        {
            var removedEntities = new HashSet<IEntity>();

            foreach (var e in entities)
            {
                if (!_loadedEntities.Remove(e))
                {
                    Logger.Warn("Unable to despawn entity on {0} (Entity not known by the client)", _client);
                    continue;
                }
                
                removedEntities.Add(e);
            }

            if (removedEntities.Count == 0)
                return 0;
            
            Logger.Info("Removing {0} entities on {1}", removedEntities.Count, _client);
            var connection = _client.Connection;
            connection.DestroyEntities(removedEntities);
            return removedEntities.Count;
        }
    }
}