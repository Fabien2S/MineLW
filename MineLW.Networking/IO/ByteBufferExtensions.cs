using System;
using System.Numerics;
using System.Text;
using DotNetty.Buffers;
using MineLW.API.Math;
using MineLW.API.Utils;
using MineLW.API.Worlds.Chunks;
using Newtonsoft.Json;

namespace MineLW.Networking.IO
{
    public static class ByteBufferExtensions
    {
        public static byte[] ToArray(this IByteBuffer buffer, out int offset, out int length)
        {
            if (buffer.HasArray)
            {
                offset = buffer.ArrayOffset;
                length = buffer.ReadableBytes;
                return buffer.Array;
            }

            offset = 0;
            length = buffer.ReadableBytes;
            var bytes = new byte[length];
            buffer.ReadBytes(bytes);
            return bytes;
        }

        public static byte[] ReadByteArray(this IByteBuffer buffer)
        {
            var bytes = new byte[buffer.ReadVarInt32()];
            buffer.ReadBytes(bytes);
            return bytes;
        }

        public static void WriteByteArray(this IByteBuffer buffer, byte[] bytes)
        {
            buffer.WriteVarInt32(bytes.Length);
            buffer.WriteBytes(bytes);
        }

        public static string ReadUtf8(this IByteBuffer buffer, short maxLen = short.MaxValue)
        {
            var len = buffer.ReadVarInt32();
            if (len > maxLen)
                throw new IndexOutOfRangeException("String is too long");

            if (buffer.HasArray)
            {
                var bytes = buffer.ReadBytes(len);
                return Encoding.UTF8.GetString(bytes.Array, bytes.ArrayOffset, len);
            }
            else
            {
                var bytes = new byte[len];
                buffer.GetBytes(buffer.ReaderIndex, bytes);
                return Encoding.UTF8.GetString(bytes);
            }
        }

        public static void WriteUtf8(this IByteBuffer buffer, string s)
        {
            if (s.Length > short.MaxValue)
                throw new IndexOutOfRangeException("String is too long");

            var bytes = Encoding.UTF8.GetBytes(s);
            buffer.WriteVarInt32(bytes.Length);
            buffer.WriteBytes(bytes);
        }

        public static void WriteIdentifier(this IByteBuffer buffer, Identifier identifier)
        {
            buffer.WriteUtf8(identifier.ToString());
        }

        public static T ReadJson<T>(this IByteBuffer buffer)
        {
            var serialized = buffer.ReadUtf8();
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public static void WriteJson(this IByteBuffer buffer, object @object)
        {
            var serialized = JsonConvert.SerializeObject(@object);
            buffer.WriteUtf8(serialized);
        }

        public static void WriteVector3F(this IByteBuffer buffer, Vector3 vector3)
        {
            buffer.WriteFloat(vector3.X);
            buffer.WriteFloat(vector3.Y);
            buffer.WriteFloat(vector3.Z);
        }

        public static void WriteVector3D(this IByteBuffer buffer, Vector3 vector3)
        {
            buffer.WriteDouble(vector3.X);
            buffer.WriteDouble(vector3.Y);
            buffer.WriteDouble(vector3.Z);
        }

        public static void WriteVector2F(this IByteBuffer buffer, Vector2 vector3)
        {
            buffer.WriteFloat(vector3.X);
            buffer.WriteFloat(vector3.Y);
        }

        public static void WriteRotation(this IByteBuffer buffer, Rotation rotation)
        {
            buffer.WriteFloat(rotation.Yaw);
            buffer.WriteFloat(rotation.Pitch);
        }

        public static void WriteChunkPosition(this IByteBuffer buffer, ChunkPosition position)
        {
            buffer.WriteInt(position.X);
            buffer.WriteInt(position.Z);
        }
    }
}