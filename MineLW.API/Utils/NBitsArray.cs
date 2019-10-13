using System;
using MineLW.API.Math;

namespace MineLW.API.Utils
{
    public class NBitsArray
    {
        public readonly long[] Backing;

        public readonly int BitsPerValue;
        public readonly int Capacity;

        private readonly long _valueMask;

        public NBitsArray(long[] backing, byte bitsPerValue, ushort capacity)
        {
            if (bitsPerValue < 1 || bitsPerValue > 32)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(bitsPerValue),
                    "Invalid bitsPerValue (either < 1 or > 32)"
                );
            }

            Backing = backing;
            BitsPerValue = bitsPerValue;
            Capacity = capacity;

            _valueMask = (1L << bitsPerValue) - 1L;
        }

        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= Capacity)
                    throw new ArgumentOutOfRangeException(nameof(index), "Invalid index");

                index *= BitsPerValue;
                var i0 = index >> 6;
                var i1 = index & 0b111111;

                var value = (long) ((ulong) Backing[i0] >> i1);
                var i2 = i1 + BitsPerValue;
                if (i2 > 64)
                    value |= Backing[++i0] << 64 - i1;
                return (int) (value & _valueMask);
            }
            set
            {
                if (index < 0 || index >= Capacity)
                    throw new ArgumentOutOfRangeException(nameof(index), "Invalid index");
                if (value < 0 || value > _valueMask)
                    throw new ArgumentOutOfRangeException(nameof(value), "Invalid value");

                index *= BitsPerValue;
                var i0 = index >> 6;
                var i1 = index & 0b111111;

                Backing[i0] = Backing[i0] & ~(_valueMask << i1) | (value & _valueMask) << i1;
                var i2 = i1 + BitsPerValue;
                // The value is divided over two long values
                if (i2 <= 64)
                    return;

                i0++;
                Backing[i0] = Backing[i0] & -(1L << i2 - 64) | ((uint) value >> 64 - i1);
            }
        }

        public static NBitsArray Create(byte bitsPerValue, ushort capacity)
        {
            return new NBitsArray(
                new long[MathHelper.RoundUp(capacity * bitsPerValue, 64) / 64],
                bitsPerValue,
                capacity
            );
        }
    }
}