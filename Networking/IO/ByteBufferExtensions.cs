using System;
using System.Text;
using DotNetty.Buffers;

namespace MineLW.Networking.IO
{
    public static class ByteBufferExtensions
    {
        public static string ReadUtf8(this IByteBuffer buffer, short maxLen = short.MaxValue)
        {
            var len = buffer.ReadVarInt32();
            if (len > maxLen)
                throw new IndexOutOfRangeException("String is too long (" + len + " > " + maxLen + ")");
            
            if (buffer.HasArray)
            {
                var bytes = buffer.ReadBytes(len);
                return Encoding.UTF8.GetString(bytes.Array, bytes.ArrayOffset, len);
            }
            else
            {
                var bytes = new byte[len];
                buffer.GetBytes(buffer.ReaderIndex, bytes);
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}