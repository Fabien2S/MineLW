using System;
using MineLW.API.Math;

namespace MineLW.API.Entities.Events
{
    public class EntityRotationChangedEventArgs : EventArgs
    {
        public readonly Rotation From;
        public readonly Rotation To;

        public EntityRotationChangedEventArgs(Rotation from, Rotation to)
        {
            From = from;
            To = to;
        }
    }
}