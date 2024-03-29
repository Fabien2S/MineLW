﻿using System;
using MineLW.API.Worlds;

namespace MineLW.API.Entities.Events
{
    public class EntityWorldChangedEventArgs : EventArgs
    {
        public readonly IWorldContext From;
        public readonly IWorldContext To;

        public EntityWorldChangedEventArgs(IWorldContext from, IWorldContext to)
        {
            From = from;
            To = to;
        }
    }
}