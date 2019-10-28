using System;

namespace MineLW.API.Worlds.Events
{
    public class WorldContextEventArgs : EventArgs
    {
        public readonly IWorldContext WorldContext;

        public WorldContextEventArgs(IWorldContext worldContext)
        {
            WorldContext = worldContext;
        }
    }
}