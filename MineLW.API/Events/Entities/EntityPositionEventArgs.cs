using System.Numerics;
using MineLW.API.Entities;

namespace MineLW.API.Events.Entities
{
    public class EntityPositionEventArgs : EntityEventArgs
    {
        public readonly Vector3 From;
        public readonly Vector3 To;
        
        public EntityPositionEventArgs(IEntity entity, Vector3 from, Vector3 to) : base(entity)
        {
            From = from;
            To = to;
        }
    }
}