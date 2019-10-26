using System.Collections.Generic;
using MineLW.API.Client;
using MineLW.API.Client.World;
using MineLW.API.Entities;
using MineLW.API.Worlds.Chunks;
using NLog;

namespace MineLW.Clients.World
{
    public class ClientEntityManager : IClientEntityManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IClient _client;
        private readonly ISet<IEntity> _loadedEntities = new HashSet<IEntity>();

        public ClientEntityManager(IClient client)
        {
            _client = client;
        }

        private ISet<IEntity> ComputeLoadedEntities()
        {
            ISet<IEntity> entities = new HashSet<IEntity>();

            var clientPlayer = _client.Player;
            var clientWorld = _client.World;
            var chunkManager = clientWorld.ChunkManager;

            var worldContexts = clientWorld.WorldContexts;
            foreach (var worldContext in worldContexts)
            {
                var entityManager = worldContext.EntityManager;
                foreach (var entity in entityManager)
                {
                    if (entity.Equals(clientPlayer))
                        continue;

                    var chunkPosition = ChunkPosition.FromWorld(entity.Position);
                    if (!chunkManager.IsLoaded(chunkPosition))
                        continue;

                    entities.Add(entity);
                }
            }

            return entities;
        }

        private bool RemoveEntities(IEnumerable<IEntity> entities)
        {
            var removedEntities = new HashSet<IEntity>();

            foreach (var e in entities)
            {
                if (_loadedEntities.Remove(e))
                    removedEntities.Add(e);
            }

            if (removedEntities.Count == 0)
                return true;

            var connection = _client.Connection;
            connection.DestroyEntities(removedEntities);
            return true;
        }

        public void SynchronizeEntities()
        {
            var toSpawn = ComputeLoadedEntities();
            var toDestroy = new HashSet<IEntity>();

            foreach (var entity in _loadedEntities)
            {
                if (toSpawn.Contains(entity))
                    toSpawn.Remove(entity);
                else
                    toDestroy.Add(entity);
            }

            RemoveEntities(toDestroy);
            foreach (var entity in toSpawn)
                SpawnEntity(entity);
        }

        public bool SpawnEntity(IEntity entity)
        {
            var player = _client.Player;
            if (player.Equals(entity))
                return false;

            var clientWorld = _client.World;
            var chunkManager = clientWorld.ChunkManager;
            var chunkPosition = ChunkPosition.FromWorld(entity.Position);
            if (!chunkManager.IsLoaded(chunkPosition))
            {
                Logger.Warn("Trying to spawn {0} in an unloaded chunk", entity);
                return false;
            }

            if (!_loadedEntities.Add(entity))
                return false;

            var clientConnection = _client.Connection;
            clientConnection.SpawnEntity(entity);
            return true;
        }

        public bool RemoveEntities(params IEntity[] entities)
        {
            return RemoveEntities((IEnumerable<IEntity>) entities);
        }
    }
}