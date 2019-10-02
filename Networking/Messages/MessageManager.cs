using System;
using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking.Messages
{
    public static class MessageManager
    {
        public static void Register<TMessage>(MessageSerializer<TMessage> serializer) where TMessage : struct
        {
            MessageSerializer<TMessage>.Instance = serializer;
        }

        public static void Register<TMessage>(MessageDeserializer<TMessage> deserializer) where TMessage : struct
        {
            MessageDeserializer<TMessage>.Instance = deserializer;
        }

        public static void Serialize<T>(IByteBuffer buffer, T message)
        {
            var serializer = MessageSerializer<T>.Instance;
            if (serializer == null)
                throw new ArgumentException("No serializer for type " + typeof(T));

            buffer.WriteVarInt32(serializer.Id);
            serializer.Serialize(buffer, message);
        }
    }
}