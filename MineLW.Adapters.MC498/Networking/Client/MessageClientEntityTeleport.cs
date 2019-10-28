using System.Numerics;
using DotNetty.Buffers;
using MineLW.API.Math;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientEntityTeleport : MessageSerializer<MessageClientEntityTeleport.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteVarInt32(message.Id);
            buffer.WriteVector3D(message.Position);
            buffer.WriteRotationB(message.Rotation);
            buffer.WriteBoolean(message.Grounded);
        }
        
        public struct Message : IMessage
        {
            public readonly int Id;
            public readonly Vector3 Position;
            public readonly Rotation Rotation;
            public readonly bool Grounded;

            public Message(int id, Vector3 position, Rotation rotation, bool grounded)
            {
                Id = id;
                Position = position;
                Rotation = rotation;
                Grounded = grounded;
            }
        }
    }
}