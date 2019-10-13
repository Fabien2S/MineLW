using System;
using System.Globalization;
using System.Text;

namespace MineLW.API.Math
{
    public struct Rotation : IEquatable<Rotation>, IFormattable
    {
        public static readonly Rotation Zero = new Rotation(0, 0);

        public readonly float Yaw;
        public readonly float Pitch;

        public Rotation(float yaw, float pitch)
        {
            Yaw = yaw % 360;
            Pitch = pitch % 360;
        }

        public bool Equals(Rotation other)
        {
            return MathHelper.AreRoughlyTheSame(Yaw, other.Yaw) && MathHelper.AreRoughlyTheSame(Yaw, other.Pitch);
        }

        public override bool Equals(object obj)
        {
            return obj is Rotation other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Yaw, Pitch).GetHashCode();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var builder = new StringBuilder();
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            builder.Append('<');
            builder.Append(((IFormattable) Yaw).ToString(format, formatProvider));
            builder.Append(separator);
            builder.Append(' ');
            builder.Append(((IFormattable) Pitch).ToString(format, formatProvider));
            builder.Append('>');
            return builder.ToString();
        }

        public static bool operator ==(Rotation a, Rotation b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Rotation a, Rotation b)
        {
            return !a.Equals(b);
        }
    }
}