﻿using System;
using System.Numerics;

namespace MineLW.API.Entities.Events
{
    public class EntityPositionChangedEventArgs : EventArgs
    {
        public readonly Vector3 From;
        public readonly Vector3 To;

        public EntityPositionChangedEventArgs(Vector3 from, Vector3 to)
        {
            From = from;
            To = to;
        }
    }
}