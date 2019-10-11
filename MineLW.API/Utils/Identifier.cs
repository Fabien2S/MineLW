using System;

namespace MineLW.API.Utils
{
    public struct Identifier : IComparable<Identifier>
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

        public override int GetHashCode()
        {
            return _hash;
        }

        public override string ToString()
        {
            return _namespace + ':' + _key;
        }
    }
}