using DotNetty.Buffers;

namespace MineLW.Networking.Messages.Serialization
{
    public interface IMessageDeserializer
    {
        IMessage Deserialize(IByteBuffer buffer);
        void Handle(MessageController controller, IMessage message);
    }

    public abstract class MessageDeserializer<TController, TMessage> : IMessageDeserializer
        where TController : MessageController where TMessage : IMessage
    {
        public abstract IMessage Deserialize(IByteBuffer buffer);
        public abstract void Handle(TController controller, TMessage message);

        public void Handle(MessageController controller, IMessage message)
        {
            if (controller is TController c && message is TMessage m)
                Handle(c, m);
        }

        public override string ToString()
        {
            return $"Message{typeof(TMessage)}";
        }
    }
}