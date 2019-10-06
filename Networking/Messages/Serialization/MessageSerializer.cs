using DotNetty.Buffers;

namespace MineLW.Networking.Messages.Serialization
{
    public interface IMessageSerializer
    {
        bool CanSerialize(IMessage message);
        void Serialize(IByteBuffer buffer, IMessage message);
    }

    public abstract class MessageSerializer<T> : IMessageSerializer where T : IMessage
    {
        public bool CanSerialize(IMessage message)
        {
            return message is T;
        }

        protected abstract void Serialize(IByteBuffer buffer, T message);

        public void Serialize(IByteBuffer buffer, IMessage message)
        {
            if (message is T t)
            {
                Serialize(buffer, t);
            }
        }

        public override string ToString()
        {
            return $"Message{typeof(T)}";
        }
    }
}