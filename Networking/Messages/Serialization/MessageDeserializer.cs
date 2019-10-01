using MineLW.Networking.IO;

namespace MineLW.Networking.Messages.Serialization
{
    public abstract class MessageDeserializer<T> where T : struct
    {
        public static MessageDeserializer<T> Instance { get; internal set; }

        public int Id { get; }

        protected MessageDeserializer(int id)
        {
            Id = id;
        }

        public abstract void Deserialize(BitStream stream);
        
        public override string ToString()
        {
            return $"Message{typeof(T)}#{Id}";
        }
    }
}