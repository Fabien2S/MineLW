using System.ComponentModel;
using MineLW.API.Worlds;

namespace MineLW.API.Entities.Events
{
    public class EntityWorldChangingEventArgs : CancelEventArgs
    {
        public readonly IWorldContext From;
        public readonly IWorldContext To;

        public EntityWorldChangingEventArgs(IWorldContext from, IWorldContext to)
        {
            From = from;
            To = to;
        }
    }
}