using DotNetty.Buffers;

namespace MineLW.API.IO
{
    public interface IByteBufferSerializable
    {
        void Serialize(IByteBuffer buffer);
    }
}