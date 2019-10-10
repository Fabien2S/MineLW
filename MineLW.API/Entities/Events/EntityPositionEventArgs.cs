using System.Numerics;
using MineLW.API.Utils;

namespace MineLW.API.Entities.Events
{
    public class EntityPositionEventArgs : EntityEventArgs, ICancellable
    {
        public bool Canceled { get; set; }
        
        public readonly Vector3 From;
        public readonly Vector3 To;
        
        public EntityPositionEventArgs(IEntity entity, Vector3 from, Vector3 to) : base(entity)
        {
            From = from;
            To = to;
        }
    }
}