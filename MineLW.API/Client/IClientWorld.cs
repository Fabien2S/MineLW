using MineLW.API.Math;

namespace MineLW.API.Client
{
    public interface IClientWorld
    {
        Vector2Int ChunkPosition { get; }
        byte RenderDistance { get; set; }

        void Init();
    }
}