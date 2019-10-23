using System;
using System.Numerics;
using MineLW.API.Entities.Events;
using MineLW.API.Math;
using MineLW.API.Utils;
using MineLW.API.Worlds;

namespace MineLW.API.Entities
{
    public interface IEntity : IUpdatable
    {
        /// <summary>
        /// The unique id of the entity. Incremental and shared across the server
        /// </summary>
        int Id { get; }

        /// <summary>
        /// The UUID of the entity
        /// </summary>
        Guid Uuid { get; }

        /// <summary>
        /// Gets if the entity is still in the world
        /// </summary>
        bool Valid { get; }

        IWorldContext WorldContext { get; set; }
        Vector3 Position { get; set; }
        Rotation Rotation { get; set; }

        event EventHandler<EntityEventArgs> Removed;
        event EventHandler<EntityWorldChangedEventArgs> WorldChanged;
        event EventHandler<EntityPositionChangedEventArgs> PositionChanged;
    }
}