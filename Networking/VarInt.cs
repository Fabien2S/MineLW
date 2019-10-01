using System;
using System.IO;
using System.Net;

namespace MineLW.Networking
{
    public static class VarInt
    {
        public const byte VarIntMaxBytes = 5;
        public const byte VarInt32BytesCount = 7;
        public const byte VarInt32IndexMask = 1 << VarInt32BytesCount;

        public static byte GetVarIntBytes(int value)
        {
            // TODO Ensure this code is correctly implemented.
            byte i;
            for (i = 0; (value & VarInt32IndexMask) != 0; i++)
            {
                value >>= VarInt32BytesCount;
                if (i > VarIntMaxBytes)
                    throw new IndexOutOfRangeException("VarInt is too big (" + i + " > " + VarIntMaxBytes + ")");
            }
            return i;
        }

        public static byte[] SerializeInt32(int value)
        {
            var bigEndianValue = IPAddress.HostToNetworkOrder(value);
            var buffer = new byte[GetVarIntBytes(bigEndianValue)];
         
            var n = 0;   
            do
            {
                var tmp = (byte) (bigEndianValue & 0b01111111);
                bigEndianValue >>= 7;
                if (bigEndianValue != 0)
                    tmp |= 0b10000000;
                buffer[n++] = tmp;
            } while (bigEndianValue != 0);

            return buffer;
        }

        public static int ReadInt32(this Stream stream)
        {
            var bytes = (byte) 0;
            var result = 0;

            byte read;
            do
            {
                read = (byte) stream.ReadByte();
                var value = (read & 0b01111111);
                result |= (value << (7 * bytes));

                bytes++;
                if (bytes > VarIntMaxBytes)
                    throw new IndexOutOfRangeException("VarInt is too big (" + bytes + " > " + VarIntMaxBytes + ")");
            } while ((read & 0b10000000) != 0);

            return IPAddress.NetworkToHostOrder(result);
        }

        public static void WriteInt32(this Stream stream, int value)
        {
            var varInt = SerializeInt32(value);
            stream.Write(varInt, 0, varInt.Length);
        }
    }
}