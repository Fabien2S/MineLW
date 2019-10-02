using DotNetty.Buffers;

namespace MineLW.Networking.Messages.Serialization
{
    public abstract class MessageSerializer<T>
    {
        public static MessageSerializer<T> Instance { get; internal set; }

        public int Id { get; }

        protected MessageSerializer(int id)
        {
            Id = id;
        }

        public abstract void Serialize(IByteBuffer buffer, object message);

        public override string ToString()
        {
            return $"Message{typeof(T)}#{Id}";
        }
    }
}