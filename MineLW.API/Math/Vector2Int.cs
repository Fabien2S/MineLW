using System;
using System.Globalization;
using System.Text;

namespace MineLW.API.Math
{
    public struct Vector2Int : IEquatable<Vector2Int>, IFormattable
    {
        public readonly int X;
        public readonly int Y;

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2Int other && Equals(other);
        }

        public bool Equals(Vector2Int other)
        {
            return X == other.X && Y == other.Y;
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
            builder.Append('>');
            return builder.ToString();
        }
    }
}