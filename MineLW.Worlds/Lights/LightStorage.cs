using MineLW.API;
using MineLW.API.Collections;
using MineLW.API.Worlds.Lights;

namespace MineLW.Worlds.Lights
{
    public class LightStorage : ILightStorage
    {
        private const int LightArraySize = Minecraft.Units.Chunk.SectionBlockCount / 2;

        private readonly NibbleArray _array = new NibbleArray(LightArraySize);

        public void SetLight(byte x, byte y, byte z, byte level)
        {
            var index = Index(x, y, z);
            _array[index] = level;
        }

        public byte GetLight(byte x, byte y, byte z)
        {
            var index = Index(x, y, z);
            return _array[index];
        }

        private static int Index(int x, int y, int z)
        {
            return (y & 0xf) << 8 | z << 4 | x;
        }
    }
}