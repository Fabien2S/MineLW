using System;

namespace MineLW.API.Entities.Events
{
    public class EntityEventArgs : EventArgs
    {
        public readonly IEntity Entity;

        public EntityEventArgs(IEntity entity)
        {
            Entity = entity;
        }
    }
}