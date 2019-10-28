using System.ComponentModel;
using MineLW.API.Math;

namespace MineLW.API.Entities.Events
{
    public class EntityRotationChangingEventArgs : CancelEventArgs
    {
        public readonly Rotation From;
        public readonly Rotation To;

        public EntityRotationChangingEventArgs(Rotation @from, Rotation to)
        {
            From = @from;
            To = to;
        }
    }
}