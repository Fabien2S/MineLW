using System;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace MineLW.API.Worlds.Chunks
{
    public struct ChunkPosition : IEquatable<ChunkPosition>, IFormattable
    {
        public readonly int X;
        public readonly int Z;

        public ChunkPosition(int x, int z)
        {
            X = x;
            Z = z;
        }

        public bool Equals(ChunkPosition other)
        {
            return X == other.X && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            return obj is ChunkPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (X, Z).GetHashCode();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var builder = new StringBuilder();
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
            builder.Append('<');
            builder.Append(((IFormattable) X).ToString(format, formatProvider));
            builder.Append(separator);
            builder.Append(' ');
            builder.Append(((IFormattable) Z).ToString(format, formatProvider));
            builder.Append('>');
            return builder.ToString();
        }

        public static bool operator ==(ChunkPosition a, ChunkPosition b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ChunkPosition a, ChunkPosition b)
        {
            return !a.Equals(b);
        }

        public static ChunkPosition FromWorld(Vector3 position)
        {
            return new ChunkPosition(
                (int) (position.X / Minecraft.Units.Chunk.Size),
                (int) (position.Z / Minecraft.Units.Chunk.Size)
            );
        }
    }
}