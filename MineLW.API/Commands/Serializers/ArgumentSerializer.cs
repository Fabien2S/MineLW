using System;
using MineLW.API.IO;

namespace MineLW.API.Commands.Serializers
{
    public interface IArgumentSerializer
    {
        string Serialize(object value);

        object Deserialize(Type type, StringReader reader);
    }

    public abstract class ArgumentSerializer<T> : IArgumentSerializer
    {
        public static ArgumentSerializer<T> Instance { get; private set; }

        public string Serialize(object value)
        {
            return Serialize((T) value);
        }

        object IArgumentSerializer.Deserialize(Type type, StringReader reader)
        {
            return Deserialize(type, reader);
        }

        public abstract string Serialize(T value);

        public abstract T Deserialize(Type type, StringReader reader);

        public static void Register<TSerializer>() where TSerializer : ArgumentSerializer<T>, new()
        {
            Instance = new TSerializer();
        }
    }
}