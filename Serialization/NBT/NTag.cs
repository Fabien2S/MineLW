namespace MineLW.Serialization.NBT
{
    public interface INTag
    {
    }
    
    public abstract class NTag<T> : INTag
    {
        public T Value => _value;
        
        private readonly T _value;

        protected NTag(T value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public static implicit operator T(NTag<T> obj)
        {
            return obj._value;
        }
    }
}