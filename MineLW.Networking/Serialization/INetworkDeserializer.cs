using DotNetty.Buffers;

namespace MineLW.Networking.Serialization
{
    public interface INetworkDeserializer
    {
        void Deserialize(IByteBuffer buffer);
    }
}