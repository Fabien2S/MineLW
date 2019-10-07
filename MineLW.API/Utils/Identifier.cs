using System;

namespace MineLW.API.Utils
{
    public struct Identifier
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

        public override string ToString()
        {
            return _namespace + ':' + _key;
        }

        public override int GetHashCode()
        {
            return _hash;
        }

        public bool Equals(Identifier other)
        {
            return _hash == other._hash;
        }

        public override bool Equals(object obj)
        {
            return obj is Identifier other && Equals(other);
        }
    }
}