using System;
using System.Text;
using DotNetty.Buffers;
using Newtonsoft.Json;

namespace MineLW.Networking.IO
{
    public static class ByteBufferExtensions
    {
        public static byte[] ToArray(this IByteBuffer buffer, out int offset, out int length)
        {
            if (buffer.HasArray)
            {
                offset = buffer.ArrayOffset;
                length = buffer.ReadableBytes;
                return buffer.Array;
            }

            offset = 0;
            length = buffer.ReadableBytes;
            var bytes = new byte[length];
            buffer.ReadBytes(bytes);
            return bytes;
        }

        public static byte[] ReadByteArray(this IByteBuffer buffer)
        {
            var bytes = new byte[buffer.ReadVarInt32()];
            buffer.ReadBytes(bytes);
            return bytes;
        }

        public static void WriteByteArray(this IByteBuffer buffer, byte[] bytes)
        {
            buffer.WriteVarInt32(bytes.Length);
            buffer.WriteBytes(bytes);
        }

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

        public static void WriteJson(this IByteBuffer buffer, object @object)
        {
            var serialized = JsonConvert.SerializeObject(@object);
            buffer.WriteUtf8(serialized);
        }
    }
}