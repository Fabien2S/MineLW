using MineLW.API.Utils;
using MineLW.API.Worlds;

namespace MineLW.API.Entities.Events
{
    public class EntityWorldChangedEventArgs : EntityEventArgs, ICancellable
    {
        public bool Canceled { get; set; }
        
        public readonly IWorldContext From;
        public readonly IWorldContext To;

        public EntityWorldChangedEventArgs(IEntity entity, IWorldContext from, IWorldContext to) : base(entity)
        {
            From = from;
            To = to;
        }
    }
}