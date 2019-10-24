using System.Numerics;
using MineLW.API.Utils;

namespace MineLW.API.Entities.Events
{
    public class EntityPositionChangedEventArgs : EntityEventArgs, ICancellable
    {
        public bool Cancelled { get; set; }
        
        public readonly Vector3 From;
        public readonly Vector3 To;
        
        public EntityPositionChangedEventArgs(IEntity entity, Vector3 from, Vector3 to) : base(entity)
        {
            From = from;
            To = to;
        }
    }
}