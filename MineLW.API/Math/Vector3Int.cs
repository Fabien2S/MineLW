using System;
using System.Globalization;
using System.Text;

namespace MineLW.API.Math
{
    public struct Vector3Int : IEquatable<Vector3Int>, IFormattable
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public Vector3Int(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override int GetHashCode()
        {
            return (X, Y, Z).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3Int other && Equals(other);
        }

        public bool Equals(Vector3Int other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var builder = new StringBuilder();
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            builder.Append('<');
            builder.Append(((IFormattable) X).ToString(format, formatProvider));
            builder.Append(separator);
            builder.Append(' ');
            builder.Append(((IFormattable) Y).ToString(format, formatProvider));
            builder.Append(separator);
            builder.Append(' ');
            builder.Append(((IFormattable) Z).ToString(format, formatProvider));
            builder.Append('>');
            return builder.ToString();
        }
    }
}