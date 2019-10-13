using System.IO;
using System.Text;
using DotNetty.Buffers;

namespace MineLW.Serialization
{
    public static class JavaSerializer
    {
        private static readonly UTF8Encoding Utf8Encoding = new UTF8Encoding(false, true);

        public static void WriteUtf8(IByteBuffer buffer, string value)
        {
            var byteCount = Utf8Encoding.GetByteCount(value);
            buffer.WriteShort(byteCount);

            var data = Utf8Encoding.GetBytes(value);
            buffer.WriteBytes(data);
        }

        public static string ReadUtf8(IByteBuffer buffer)
        {
            var length = buffer.ReadShort();

            var data = new byte[length];
            buffer.ReadBytes(data);
            if (data.Length < length)
                throw new EndOfStreamException();

            return Utf8Encoding.GetString(data);
        }
    }
}