using System;
using MineLW.API.Entities.Events;
using MineLW.API.Entities.Living;
using MineLW.API.Math;

namespace MineLW.Entities.Living
{
    public abstract class EntityLiving : Entity, IEntityLiving
    {
        public event EventHandler<EntityRotationChangingEventArgs> HeadAngleChanging;
        public event EventHandler<EntityRotationChangedEventArgs> HeadAngleChanged;

        public float HeadAngle
        {
            get => _headAngle;
            set
            {
                var entityRotation = Rotation;

                var previousRotation = new Rotation(_headAngle, entityRotation.Pitch);
                var currentRotation = new Rotation(value, entityRotation.Pitch);
                var rotationChangingEventArgs = new EntityRotationChangingEventArgs(previousRotation, currentRotation);
                HeadAngleChanging?.Invoke(this, rotationChangingEventArgs);
                if (rotationChangingEventArgs.Cancel)
                    return;

                _headAngle = value;

                var rotationChangedEventArgs = new EntityRotationChangedEventArgs(previousRotation, currentRotation);
                HeadAngleChanged?.Invoke(this, rotationChangedEventArgs);
            }
        }

        private float _headAngle;

        protected EntityLiving(int id, Guid uuid) : base(id, uuid)
        {
        }
    }
}