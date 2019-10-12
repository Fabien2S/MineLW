using DotNetty.Buffers;

namespace MineLW.Networking.Serialization
{
    public interface INetworkSerializer
    {
        void Serialize(IByteBuffer buffer);
    }
}