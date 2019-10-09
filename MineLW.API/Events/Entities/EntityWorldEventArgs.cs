using MineLW.API.Entities;
using MineLW.API.Worlds.Context;

namespace MineLW.API.Events.Entities
{
    public class EntityWorldEventArgs : EntityEventArgs
    {
        public readonly IWorldContext From;
        public readonly IWorldContext To;

        public EntityWorldEventArgs(IEntity entity, IWorldContext from, IWorldContext to) : base(entity)
        {
            From = from;
            To = to;
        }
    }
}