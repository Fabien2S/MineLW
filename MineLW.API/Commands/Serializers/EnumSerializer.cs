using System;
using System.ComponentModel;
using MineLW.API.Commands.Exceptions;
using MineLW.API.IO;

namespace MineLW.API.Commands.Serializers
{
    public class EnumSerializer : ArgumentSerializer<Enum>
    {
        public override string Serialize(Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null)
                throw new InvalidEnumArgumentException("Invalid enum " + value);
            return name.ToLowerInvariant();
        }

        public override Enum Deserialize(Type type, StringReader reader)
        {
            var enumValue = reader.ReadString();
            if (Enum.TryParse(type, enumValue, true, out var e))
                return (Enum) e;
            throw new CommandParseException("Invalid enum value (" + enumValue + ')');
        }
    }
}