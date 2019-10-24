using System;
using System.Globalization;
using MineLW.API.IO;

namespace MineLW.API.Commands.Serializers
{
    public class IntSerializer : ArgumentSerializer<int>
    {
        public override string Serialize(int value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        public override int Deserialize(Type type, StringReader reader)
        {
            return reader.ReadInteger();
        }
    }
}