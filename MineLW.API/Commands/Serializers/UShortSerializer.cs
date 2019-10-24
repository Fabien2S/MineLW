using System;
using System.Globalization;
using MineLW.API.IO;

namespace MineLW.API.Commands.Serializers
{
    public class UShortSerializer : ArgumentSerializer<ushort>
    {
        public override string Serialize(ushort value)
        {
            return value.ToString(NumberFormatInfo.InvariantInfo);
        }

        public override ushort Deserialize(Type type, StringReader reader)
        {
            return (ushort) reader.ReadInteger();
        }
    }
}