using DotNetty.Buffers;

namespace MineLW.Serialization.NBT
{
    public interface INbtTag
    {
        byte Id { get; }
        void Serialize(IByteBuffer buffer);
    }
    
    public abstract class NbtTag<T> : INbtTag
    {
        public abstract byte Id { get; }

        public T Value => _value;
        
        private readonly T _value;

        protected NbtTag(T value)
        {
            _value = value;
        }
        
        public abstract void Serialize(IByteBuffer buffer);

        public override string ToString()
        {
            return _value.ToString();
        }

        public static implicit operator T(NbtTag<T> obj)
        {
            return obj._value;
        }
    }
}