using MineLW.API.Math;

namespace MineLW.API.Worlds.Lights
{
    public interface ILightManager
    {
        void SetLight(Vector3Int position, LightType type, byte level);
        byte GetLight(Vector3Int position, LightType type);
        byte ComputeLight(Vector3Int position);
    }
}