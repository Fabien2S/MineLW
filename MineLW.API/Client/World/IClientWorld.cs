using MineLW.API.Utils;
using MineLW.API.Worlds.Chunks;

namespace MineLW.API.Client.World
{
    public interface IClientWorld : IUpdatable
    {
        IClientChunkManager ChunkManager { get; }
        IClientEntityManager EntityManager { get; }
        
        ChunkPosition ChunkPosition { get; set; }
        
        byte RenderDistance { get; set; }

        void Init();
    }
}