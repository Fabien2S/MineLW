using System;
using MineLW.API.IO;

namespace MineLW.API.Commands.Serializers
{
    public class StringSerializer : ArgumentSerializer<string>
    {
        public override string Serialize(string value)
        {
            return value;
        }

        public override string Deserialize(Type type, StringReader reader)
        {
            return reader.ReadString();
        }
    }
}