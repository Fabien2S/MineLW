using System;
using System.Collections.Generic;
using MineLW.API;
using MineLW.API.Math;
using MineLW.API.Worlds.Lights;

namespace MineLW.Worlds.Lights
{
    public class LightManager : ILightManager
    {
        public const byte MinLightLevel = 0;
        public const byte MaxLightLevel = 15;

        public const int SectionCount = Minecraft.Units.Chunk.SectionCount + 2;
        
        private static readonly LightType[] Values = (LightType[]) Enum.GetValues(typeof(LightType));
        
        private readonly Dictionary<LightType, ILightStorage> _lightStorage = new Dictionary<LightType, ILightStorage>();

        public LightManager()
        {
            foreach (var type in Values)
                _lightStorage[type] = new LightStorage();
        }

        public void SetLight(Vector3Int position, LightType type, byte level)
        {
            var storage = _lightStorage[type];
            storage.SetLight((byte) position.X, (byte) position.Y, (byte) position.Z, level);
        }

        public byte GetLight(Vector3Int position, LightType type)
        {
            var storage = _lightStorage[type];
            return storage.GetLight((byte) position.X, (byte) position.Y, (byte) position.Z);
        }

        public byte ComputeLight(Vector3Int position)
        {
            throw new NotSupportedException();
        }
    }
}