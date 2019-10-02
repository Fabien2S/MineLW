using DotNetty.Buffers;

namespace MineLW.Networking.Messages.Serialization
{
    public abstract class MessageDeserializer<T>
    {
        public static MessageDeserializer<T> Instance { get; internal set; }

        public int Id { get; }

        protected MessageDeserializer(int id)
        {
            Id = id;
        }

        public abstract T Deserialize(IByteBuffer buffer);

        public override string ToString()
        {
            return $"Message{typeof(T)}#{Id}";
        }
    }
}