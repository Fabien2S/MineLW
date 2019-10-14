using System.Collections.Generic;
using MineLW.API.Utils;
using MineLW.API.Worlds;
using MineLW.API.Worlds.Chunks;

namespace MineLW.API.Client.World
{
    public interface IClientWorld : IUpdatable
    {
        IClientChunkManager ChunkManager { get; }
        IClientEntityManager EntityManager { get; }

        ChunkPosition ChunkPosition { get; set; }
        IEnumerable<IWorldContext> WorldContexts { get; }

        byte RenderDistance { get; set; }

        void Init();
        void RegisterContext(IWorldContext context);
        void UnregisterContext(IWorldContext context);
    }
}