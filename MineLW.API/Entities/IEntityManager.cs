using System;
using System.Collections.Generic;
using System.Numerics;
using MineLW.API.Client;
using MineLW.API.Entities.Events;
using MineLW.API.Entities.Living.Player;
using MineLW.API.Math;
using MineLW.API.Utils;
using MineLW.API.Worlds.Chunks;

namespace MineLW.API.Entities
{
    public interface IEntityManager : IUpdatable, IEnumerable<IEntity>
    {
        event EventHandler<EntityEventArgs> EntitySpawned;

        IEnumerable<IEntity> GetEntities(ChunkPosition position);
        
        /// <summary>
        /// Spawn an entity in the world context 
        /// </summary>
        /// <param name="name">The entity identifier</param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns>The spawned entity</returns>
        /// <exception cref="System.ArgumentException"></exception>
        IEntity SpawnEntity(Identifier name, Vector3 position, Rotation rotation);

        /// <summary>
        /// Spawn a client-entity in the world context
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns>The spawned client entity</returns>
        IEntityPlayer SpawnPlayer(IClient client, Vector3 position, Rotation rotation);
    }
}