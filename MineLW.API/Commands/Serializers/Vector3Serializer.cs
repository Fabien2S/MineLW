using System;
using System.Numerics;
using MineLW.API.IO;

namespace MineLW.API.Commands.Serializers
{
    public class Vector3Serializer : ArgumentSerializer<Vector3>
    {
        public override string Serialize(Vector3 value)
        {
            return value.X + " " + value.Y + " " + value.Z;
        }

        public override Vector3 Deserialize(Type type, StringReader reader)
        {
            var x = reader.ReadFloat();
            reader.ConsumeWhitespaces();

            var y = reader.ReadFloat();
            reader.ConsumeWhitespaces();

            var z = reader.ReadFloat();

            return new Vector3(x, y, z);
        }
    }
}