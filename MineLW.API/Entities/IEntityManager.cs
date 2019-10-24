using System.Collections.Generic;
using System.Numerics;
using MineLW.API.Math;
using MineLW.API.Utils;

namespace MineLW.API.Entities
{
    public interface IEntityManager : IUpdatable, IEnumerable<IEntity>
    {
        /// <summary>
        /// Spawn an entity in the world context 
        /// </summary>
        /// <param name="name">The entity identifier</param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns>The spawned entity</returns>
        /// <exception cref="System.ArgumentException"></exception>
        IEntity SpawnEntity(Identifier name, Vector3 position, Rotation rotation);
    }
}