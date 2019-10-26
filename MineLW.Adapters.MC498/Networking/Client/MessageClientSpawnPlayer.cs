using System;
using System.Numerics;
using DotNetty.Buffers;
using MineLW.API.Math;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientSpawnPlayer : MessageSerializer<MessageClientSpawnPlayer.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteVarInt32(message.Id);
            buffer.WriteGuid(message.Guid);
            buffer.WriteVector3D(message.Position);
            buffer.WriteRotation(message.Rotation);
            buffer.WriteByte(0xFF);
        }
        
        public struct Message : IMessage
        {
            public readonly int Id;
            public readonly Guid Guid;
            public readonly Vector3 Position;
            public readonly Rotation Rotation;

            public Message(int id, Guid guid, Vector3 position, Rotation rotation)
            {
                Id = id;
                Guid = guid;
                Position = position;
                Rotation = rotation;
            }
        }
    }
}