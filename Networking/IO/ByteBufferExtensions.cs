using System;
using System.Text;
using System.Text.Json;
using DotNetty.Buffers;

namespace MineLW.Networking.IO
{
    public static class ByteBufferExtensions
    {
        public static string ReadUtf8(this IByteBuffer buffer, short maxLen = short.MaxValue)
        {
            var len = buffer.ReadVarInt32();
            if (len > maxLen)
                throw new IndexOutOfRangeException("String is too long");

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

        public static void WriteUtf8(this IByteBuffer buffer, string s)
        {
            if (s.Length > short.MaxValue)
                throw new IndexOutOfRangeException("String is too long");

            var bytes = Encoding.UTF8.GetBytes(s);
            buffer.WriteVarInt32(bytes.Length);
            buffer.WriteBytes(bytes);
        }

        public static void WriteJson(this IByteBuffer buffer, object @object, JsonSerializerOptions options = null)
        {
            var serialized = JsonSerializer.Serialize(@object, options);
            buffer.WriteUtf8(serialized);
        }
    }
}