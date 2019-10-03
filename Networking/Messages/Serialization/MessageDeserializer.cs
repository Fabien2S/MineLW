using DotNetty.Buffers;

namespace MineLW.Networking.Messages.Serialization
{
    public interface IMessageDeserializer
    {
        IMessage Deserialize(IByteBuffer buffer);
    }

    public abstract class MessageDeserializer<T> : IMessageDeserializer where T : IMessage
    {
        public abstract IMessage Deserialize(IByteBuffer buffer);

        public override string ToString()
        {
            return $"Message{typeof(T)}";
        }
    }
}