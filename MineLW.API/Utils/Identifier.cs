using System;

namespace MineLW.API.Utils
{
    public struct Identifier : IEquatable<Identifier>, IComparable<Identifier>
    {
        private readonly string _namespace;
        private readonly string _key;

        [NonSerialized] private readonly int _hash;

        public Identifier(string @namespace, string key)
        {
            _namespace = @namespace;
            _key = key;
            _hash = (@namespace, key).GetHashCode();
        }

        public int CompareTo(Identifier other)
        {
            return _hash.CompareTo(other._hash);
        }

        public bool Equals(Identifier other)
        {
            return _hash == other._hash;
        }

        public override bool Equals(object obj)
        {
            return obj is Identifier other && Equals(other);
        }

        public override int GetHashCode() => _hash;

        public override string ToString() => _namespace + ':' + _key;

        public static bool operator ==(Identifier a, Identifier b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Identifier a, Identifier b)
        {
            return !a.Equals(b);
        }
    }
}