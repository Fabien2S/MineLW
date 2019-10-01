using MineLW.Networking.IO;

namespace MineLW.Networking
{
    public interface INetworkDeserializer
    {
        void Deserialize(BitStream stream);
    }
}