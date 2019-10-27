using System.ComponentModel;
using System.Numerics;

namespace MineLW.API.Entities.Events
{
    public class EntityPositionChangingEventArgs : CancelEventArgs
    {
        public readonly Vector3 From;
        public readonly Vector3 To;

        public EntityPositionChangingEventArgs(Vector3 from, Vector3 to)
        {
            From = from;
            To = to;
        }
    }
}