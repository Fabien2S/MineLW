using System;
using MineLW.API.Blocks.Properties;

namespace MineLW.Blocks.Properties
{
    public abstract class BlockProperty<T> : IBlockProperty
    {
        public string Name { get; }
        public int ValueCount => Values.Length;

        protected readonly T[] Values;

        protected BlockProperty(string name, T[] values)
        {
            Name = name;
            Values = values;
        }

        public abstract object Parse(string source);

        public int GetIndex(object value)
        {
            return value is T t ? Array.IndexOf(Values, t) : -1;
        }

        public object GetValue(int index) => Values[index];

        private bool Equals(BlockProperty<T> other)
        {
            return string.Equals(Name, other.Name) && Equals(Values, other.Values);
        }

        public override bool Equals(object obj)
        {
            return obj is BlockProperty<T> other && Equals(other);
        }

        public override int GetHashCode() => (Name, Values).GetHashCode();

        public override string ToString() => Name + '[' + string.Join(", ", Values) + ']';
    }
}