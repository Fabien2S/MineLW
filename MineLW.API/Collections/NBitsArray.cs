using System;
using MineLW.API.Math;

namespace MineLW.API.Collections
{
    public class NBitsArray
    {
        public readonly long[] Backing;

        public readonly int BitsPerEntry;
        public readonly int Capacity;

        private readonly long _maxValue;

        private NBitsArray(long[] backing, byte bitsPerEntry, ushort capacity)
        {
            if (bitsPerEntry < 1 || bitsPerEntry > 32)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(bitsPerEntry),
                    bitsPerEntry,
                    "Invalid bit count per entry"
                );
            }

            Backing = backing;
            BitsPerEntry = bitsPerEntry;
            Capacity = capacity;

            _maxValue = (1L << bitsPerEntry) - 1L;
        }

        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= Capacity)
                    throw new ArgumentOutOfRangeException(nameof(index), "Invalid index");

                index *= BitsPerEntry;
                var i0 = index >> 6;
                var i1 = index & 0x3f;

                var value = (long) ((ulong) Backing[i0] >> i1);
                var i2 = i1 + BitsPerEntry;
                if (i2 > 64)
                    value |= Backing[++i0] << 64 - i1;
                return (int) (value & _maxValue);
            }
            set
            {
                if (index < 0 || index >= Capacity)
                    throw new ArgumentOutOfRangeException(nameof(index), "Invalid index");
                if (value < 0 || value > _maxValue)
                    throw new ArgumentOutOfRangeException(nameof(value), "Invalid value");

                index *= BitsPerEntry;
                var i0 = index >> 6;
                var i1 = index & 0x3f;

                Backing[i0] = Backing[i0] & ~(_maxValue << i1) | (value & _maxValue) << i1;
                var i2 = i1 + BitsPerEntry;
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