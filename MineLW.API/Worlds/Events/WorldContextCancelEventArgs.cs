using System.ComponentModel;

namespace MineLW.API.Worlds.Events
{
    public class WorldContextCancelEventArgs : CancelEventArgs
    {
        public readonly IWorldContext WorldContext;

        public WorldContextCancelEventArgs(IWorldContext worldContext)
        {
            WorldContext = worldContext;
        }
    }
}