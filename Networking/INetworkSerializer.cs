using MineLW.Networking.IO;

namespace MineLW.Networking
{
    public interface INetworkSerializer
    {
        void Serialize(BitStream stream);
    }
}