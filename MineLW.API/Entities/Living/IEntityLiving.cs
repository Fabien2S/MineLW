using System;
using MineLW.API.Entities.Events;

namespace MineLW.API.Entities.Living
{
    public interface IEntityLiving : IEntity
    {
        event EventHandler<EntityRotationChangingEventArgs> HeadAngleChanging;
        event EventHandler<EntityRotationChangedEventArgs> HeadAngleChanged;
        
        float HeadAngle { get; set; }
    }
}