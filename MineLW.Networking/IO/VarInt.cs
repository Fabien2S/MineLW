using System.IO;
using DotNetty.Buffers;

namespace MineLW.Networking.IO
{
    public static class VarInt
    {
        private const byte VarIntIndexMask = 0b10000000;
        private const byte VarIntContentMask = 0b01111111;
        private const byte VarIntContentBytesCount = 7;

        public const byte VarInt32MaxBytes = 5;

        public static bool TryReadVarInt32(this IByteBuffer buffer, out int result)
        {
            buffer.MarkReaderIndex();

            var numBytes = (byte) 0;

            result = 0;
            byte read;
            do
            {
                if (!buffer.IsReadable())
                {
                    buffer.ResetReaderIndex();
                    return false;
                }

                read = buffer.ReadByte();
                var value = read & VarIntContentMask;
                result |= value << (VarIntContentBytesCount * numBytes);

                numBytes++;
                if (numBytes <= VarInt32MaxBytes)
                    continue;
                
                buffer.ResetReaderIndex();
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

        public static void WriteVarInt32(this IByteBuffer buffer, int i)
        {
            do
            {
                var temp = (byte) (i & VarIntContentMask);
                i = (int) ((uint) i >> VarIntContentBytesCount);
                if (i != 0)
                    temp |= VarIntIndexMask;
                buffer.WriteByte(temp);
            } while (i != 0);
        }
    }
}