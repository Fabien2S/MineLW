using System.Collections.ObjectModel;
using MineLW.API.Blocks.Properties;

namespace MineLW.Blocks.Properties
{
    public abstract class BlockProperty<T> : IBlockProperty
    {
        public string Name { get; }
        public int ValueCount => Values.Count;

        protected readonly ReadOnlyCollection<T> Values;

        protected BlockProperty(string name, ReadOnlyCollection<T> values)
        {
            Name = name;
            Values = values;
        }

        public abstract object Parse(string source);

        public int GetIndex(object value)
        {
            return value is T t ? Values.IndexOf(t) : -1;
        }

        public object GetValue(int index)
        {
            return Values[index];
        }

        public override string ToString()
        {
            return Name + '[' + string.Join(", ", Values) + ']';
        }

        protected bool Equals(BlockProperty<T> other)
        {
            return string.Equals(Name, other.Name) && Equals(Values, other.Values);
        }

        public override bool Equals(object obj)
        {
            return obj is BlockProperty<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Values.GetHashCode() * 397) ^ Name.GetHashCode();
        }
    }
}