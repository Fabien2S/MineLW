using DotNetty.Buffers;

namespace MineLW.Networking.Messages.Serialization
{
    public interface IMessageSerializer
    {
        void Serialize(IByteBuffer buffer, IMessage message);
    }

    public abstract class MessageSerializer<T> : IMessageSerializer where T : IMessage
    {
        public abstract void Serialize(IByteBuffer buffer, T message);

        public void Serialize(IByteBuffer buffer, IMessage message)
        {
            if (message is T t)
                Serialize(buffer, t);
        }

        public override string ToString()
        {
            return $"Message{typeof(T)}";
        }
    }
}