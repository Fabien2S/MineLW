using System.Collections.ObjectModel;
using MineLW.API.Blocks.Properties;

namespace MineLW.Blocks.Properties
{
    public abstract class BlockProperty<T> : IBlockProperty
    {
        public string Name { get; }

        protected readonly ReadOnlyCollection<T> Values;

        protected BlockProperty(string name, ReadOnlyCollection<T> values)
        {
            Name = name;
            this.Values = values;
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

        public int GetValueCount()
        {
            return Values.Count;
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
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BlockProperty<T>) obj);
        }

        public override int GetHashCode()
        {
            return (Values.GetHashCode() * 397) ^ Name.GetHashCode();
        }
    }
}