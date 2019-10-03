using System.IO;
using System.Net;
using DotNetty.Buffers;

namespace MineLW.Networking.IO
{
    public static class VarInt
    {
        private const byte VarIntIndexMask = 0b10000000;
        private const byte VarIntContentMask = 0b01111111;
        private const byte VarIntContentBytesCount = 7;
        
        private const byte VarInt32MaxBytes = 5;

        public static bool TryReadVarInt32(this IByteBuffer buffer, out int result)
        {
            var numBytes = (byte) 0;

            result = 0;
            byte read;
            do
            {
                if (buffer.ReadableBytes < 1)
                    return false;

                read = buffer.ReadByte();
                var value = read & VarIntContentMask;
                result |= value << (VarIntContentBytesCount * numBytes);

                numBytes++;
                if (numBytes > VarInt32MaxBytes)
                    return false;
                
            } while ((read & VarIntIndexMask) != 0);

            return true;
        }

        public static int ReadVarInt32(this IByteBuffer buffer)
        {
            if (TryReadVarInt32(buffer, out var result))
                return result;
            throw new IOException("Invalid VarInt32");
        }

        public static void WriteVarInt32(this IByteBuffer buffer, int value)
        {
            var bigEndianValue = IPAddress.HostToNetworkOrder(value);

            do
            {
                var tmp = (byte) (bigEndianValue & VarIntContentMask);
                bigEndianValue >>= VarIntContentBytesCount;
                if (bigEndianValue != 0)
                    tmp |= VarIntIndexMask;
                buffer.WriteByte(tmp);
            } while (bigEndianValue != 0);
        }
    }
}