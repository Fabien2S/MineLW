using System;

namespace MineLW.API.Worlds.Events
{
    public class WorldEventArgs : EventArgs
    {
        public readonly IWorld World;

        public WorldEventArgs(IWorld world)
        {
            World = world;
        }
    }
}