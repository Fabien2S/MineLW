using MineLW.API.Math;
using MineLW.API.Worlds;

namespace MineLW.API.Client
{
    public interface IClientWorld
    {
        Vector2Int ChunkPosition { get; }
        IWorldContext WorldContext { get; }
        byte RenderDistance { get; set; }
    }
}