using System;

namespace MineLW.API.Utils
{
    [Serializable]
    public struct GameVersion : IComparable<GameVersion>
    {
        public readonly string Name;
        public readonly int Protocol;

        public GameVersion(string name, int protocol)
        {
            Name = name;
            Protocol = protocol;
        }

        public bool Equals(GameVersion other)
        {
            return Protocol == other.Protocol;
        }

        public override bool Equals(object obj)
        {
            return obj is GameVersion other && Equals(other);
        }

        public int CompareTo(GameVersion other)
        {
            return Protocol.CompareTo(other.Protocol);
        }

        public override int GetHashCode()
        {
            return Protocol;
        }

        public override string ToString()
        {
            return Name + '[' + Protocol + ']';
        }

        public static bool operator >(GameVersion a, GameVersion b)
        {
            return a.Protocol > b.Protocol;
        }

        public static bool operator <(GameVersion a, GameVersion b)
        {
            return a.Protocol < b.Protocol;
        }

        public static bool operator >=(GameVersion a, GameVersion b)
        {
            return a.Protocol >= b.Protocol;
        }

        public static bool operator <=(GameVersion a, GameVersion b)
        {
            return a.Protocol <= b.Protocol;
        }

        public static bool operator ==(GameVersion a, GameVersion b)
        {
            return a.Protocol == b.Protocol;
        }

        public static bool operator !=(GameVersion a, GameVersion b)
        {
            return a.Protocol != b.Protocol;
        }
    }
}