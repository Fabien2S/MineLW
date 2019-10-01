using MineLW.Networking.IO;

namespace MineLW.Networking.Messages.Serialization
{
    public abstract class MessageSerializer<T> : IMessageProcessor where T : struct
    {
        public static MessageSerializer<T> Instance { get; internal set; }

        public int Id { get; }

        protected MessageSerializer(int id)
        {
            Id = id;
        }

        public abstract void Serialize(BitStream stream, object message);
        
        public override string ToString()
        {
            return $"Message{typeof(T)}#{Id}";
        }
    }
}