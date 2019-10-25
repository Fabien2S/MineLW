using System;
using System.Numerics;
using MineLW.API.Entities;
using MineLW.API.Entities.Events;
using MineLW.API.Math;
using MineLW.API.Worlds;

namespace MineLW.Entities
{
    public class Entity : IEntity
    {
        public int Id { get; }
        public Guid Uuid { get; }
        public bool Valid { get; private set; } = true;

        public IWorldContext WorldContext
        {
            get => _worldContext;
            set
            {
                EnsureValid();

                var worldEventArgs = new EntityWorldChangedEventArgs(this, _worldContext, value);
                WorldChanged?.Invoke(this, worldEventArgs);
                if (worldEventArgs.Cancelled)
                    return;

                _worldContext = value;
            }
        }

        public Vector3 Position
        {
            get => _position;
            set
            {
                EnsureValid();

                var positionEventArgs = new EntityPositionChangedEventArgs(this, _position, value);
                PositionChanged?.Invoke(this, positionEventArgs);
                if (!positionEventArgs.Cancelled)
                    _position = value;
            }
        }

        public Rotation Rotation
        {
            get => _rotation;
            set
            {
                EnsureValid();
                _rotation = value;
            }
        }

        public event EventHandler<EntityEventArgs> Removed;
        public event EventHandler<EntityWorldChangedEventArgs> WorldChanged;
        public event EventHandler<EntityPositionChangedEventArgs> PositionChanged;

        private IWorldContext _worldContext;
        private Vector3 _position = Vector3.Zero;
        private Rotation _rotation = Rotation.Zero;

        protected Entity(int id, Guid uuid)
        {
            Id = id;
            Uuid = uuid;
        }

        protected void EnsureValid()
        {
            if (Valid)
                return;

            throw new InvalidOperationException(
                "The entity isn't valid. You are probably referring to a removed entity"
            );
        }

        public virtual void Update(float deltaTime)
        {
        }

        public void Remove()
        {
            EnsureValid();

            Removed?.Invoke(this, new EntityEventArgs(this));
            Valid = false;
        }

        public override bool Equals(object obj) => obj is IEntity other && Equals(other);
        public bool Equals(IEntity other) => Id == other?.Id;
        public override int GetHashCode() => Id;
    }
}