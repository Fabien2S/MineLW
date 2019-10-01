using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking.Messages
{
    public static class MessageRegistry
    {
        public static void Register<TMessage>(MessageSerializer<TMessage> serializer) where TMessage : struct
        {
            MessageSerializer<TMessage>.Instance = serializer;
        }

        public static void Register<TMessage>(MessageDeserializer<TMessage> deserializer) where TMessage : struct
        {
            MessageDeserializer<TMessage>.Instance = deserializer;
        }
    }
}