using MineLW.API.Utils;
using MineLW.API.Worlds;

namespace MineLW.API.Entities.Events
{
    public class EntityWorldEventArgs : EntityEventArgs, ICancellable
    {
        public bool Canceled { get; set; }
        
        public readonly IWorldContext From;
        public readonly IWorldContext To;

        public EntityWorldEventArgs(IEntity entity, IWorldContext from, IWorldContext to) : base(entity)
        {
            From = from;
            To = to;
        }
    }
}