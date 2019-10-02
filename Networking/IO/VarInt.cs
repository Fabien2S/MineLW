using System.IO;
using System.Net;
using DotNetty.Buffers;

namespace MineLW.Networking.IO
{
    public static class VarInt
    {
        public const byte VarIntMaxBytes = 5;

        private const byte VarInt32BytesCount = 7;
        private const byte VarInt32IndexMask = 1 << VarInt32BytesCount;
        private const byte VarInt32ContentMask = byte.MaxValue & ~VarInt32IndexMask;

        public static bool TryReadVarInt(this IByteBuffer buffer, out int result)
        {
            var bytes = (byte) 0;

            result = 0;
            byte read;
            do
            {
                if (buffer.ReadableBytes < 1)
                    return false;

                read = buffer.ReadByte();
                var value = read & VarInt32ContentMask;
                result |= value << (VarInt32BytesCount * bytes);

                bytes++;
                if (bytes > VarIntMaxBytes)
                    return false;
            } while ((read & VarInt32IndexMask) != 0);

            return true;
        }

        public static int ReadVarInt32(this IByteBuffer buffer)
        {
            if (TryReadVarInt(buffer, out var result))
                return result;
            throw new IOException("Invalid VarInt32");
        }

        public static void WriteVarInt32(this IByteBuffer buffer, int value)
        {
            var bigEndianValue = IPAddress.HostToNetworkOrder(value);

            do
            {
                var tmp = (byte) (bigEndianValue & VarInt32ContentMask);
                bigEndianValue >>= VarInt32BytesCount;
                if (bigEndianValue != 0)
                    tmp |= VarInt32IndexMask;
                buffer.WriteByte(tmp);
            } while (bigEndianValue != 0);
        }
    }
}