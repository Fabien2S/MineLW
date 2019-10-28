using System;
using System.Numerics;
using MineLW.API.Entities;
using MineLW.API.Entities.Events;
using MineLW.API.Math;
using MineLW.API.Text;
using MineLW.API.Worlds;

namespace MineLW.Entities
{
    public class Entity : IEntity
    {
        public int Id { get; }
        public Guid Uuid { get; }
        public bool Valid { get; private set; } = true;
        
        public TextComponent DisplayName { get; set; }

        public IWorldContext WorldContext
        {
            get => _worldContext;
            set
            {
                EnsureValid();
                
                var previousWorldContext = _worldContext;
                
                var worldChangingEventArgs = new EntityWorldChangingEventArgs(previousWorldContext, value);
                WorldChanging?.Invoke(this, worldChangingEventArgs);
                if (worldChangingEventArgs.Cancel)
                    return;

                _worldContext = value;
                
                var worldChangedEventArgs = new EntityWorldChangedEventArgs(previousWorldContext, value);
                WorldChanged?.Invoke(this, worldChangedEventArgs);
            }
        }

        public Vector3 Position
        {
            get => _position;
            set
            {
                EnsureValid();
                
                var previousPosition = _position;

                var positionChangingEventArgs = new EntityPositionChangingEventArgs(previousPosition, value);
                PositionChanging?.Invoke(this, positionChangingEventArgs);
                if (positionChangingEventArgs.Cancel)
                    return;
                _position = value;
                    
                var positionChangedEventArgs = new EntityPositionChangedEventArgs(previousPosition, value);
                PositionChanged?.Invoke(this, positionChangedEventArgs);
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

        public event EventHandler Removed;
        public event EventHandler<EntityWorldChangingEventArgs> WorldChanging;
        public event EventHandler<EntityWorldChangedEventArgs> WorldChanged;
        public event EventHandler<EntityPositionChangingEventArgs> PositionChanging;
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

            Removed?.Invoke(this, EventArgs.Empty);
            Valid = false;
        }

        public override bool Equals(object obj) => obj is IEntity other && Equals(other);
        public bool Equals(IEntity other) => Id == other?.Id;
        public int CompareTo(IEntity other) => Id.CompareTo(other.Id);
        public override int GetHashCode() => Id;
    }
}