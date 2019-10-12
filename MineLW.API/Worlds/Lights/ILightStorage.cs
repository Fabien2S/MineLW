namespace MineLW.API.Worlds.Lights
{
    public interface ILightStorage
    {
        void SetLight(byte x, byte y, byte z, byte level);
        byte GetLight(byte x, byte y, byte z);
    }
}