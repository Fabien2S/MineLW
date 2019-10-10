using System;
using System.Numerics;
using MineLW.API.Entities;
using MineLW.API.Entities.Events;
using MineLW.API.Worlds.Context;

namespace MineLW.Entities
{
    public class Entity : IEntity
    {
        public int Id { get; }
        public Guid Uuid { get; }
        public bool Valid { get; private set; } = true;

        public IWorldContext WorldContext { get; set; }
        public Vector3 Position { get; set; } = Vector3.Zero;
        public Vector2 Rotation { get; set; } = Vector2.Zero;

        public event EventHandler<EntityEventArgs> Removed;
        public event EventHandler<EntityWorldEventArgs> WorldChanged;
        public event EventHandler<EntityPositionEventArgs> PositionChanged;

        public Entity(int id, Guid uuid)
        {
            Id = id;
            Uuid = uuid;
        }

        private void EnsureValid()
        {
            if (!Valid)
                throw new InvalidOperationException(
                    "The entity isn't valid. You are probably referring to a removed entity");
        }

        public virtual void Update(float deltaTime)
        {
        }

        public void Move(IWorldContext worldContext, Vector3 position = default, Vector2 rotation = default)
        {
            EnsureValid();

            if (position != default && Position != position)
            {
                var positionEventArgs = new EntityPositionEventArgs(this, Position, position);
                PositionChanged?.Invoke(this, positionEventArgs);
                if (!positionEventArgs.Canceled)
                    Position = position;
            }

            if (rotation != default && Rotation != rotation)
            {
                Rotation = rotation;
            }

            if (WorldContext == worldContext)
                return;

            var worldEventArgs = new EntityWorldEventArgs(this, WorldContext, worldContext);
            WorldChanged?.Invoke(this, worldEventArgs);
            if (worldEventArgs.Canceled)
                return;

            WorldContext?.EntityManager.RemoveEntity(this);
            WorldContext = worldContext;
            WorldContext.EntityManager.AddEntity(this);
        }

        public void Remove()
        {
            if (!Valid)
                return;

            Removed?.Invoke(this, new EntityEventArgs(this));
            Valid = false;
        }

        public override bool Equals(object obj) => obj is IEntity other && Equals(other);
        private bool Equals(IEntity other) => Id == other.Id;
        public override int GetHashCode() => Id;
    }
}