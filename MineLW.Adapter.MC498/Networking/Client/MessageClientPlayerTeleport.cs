using System.Numerics;
using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapter.MC498.Networking.Networking.Client
{
    public class MessageClientPlayerTeleport : MessageSerializer<MessageClientPlayerTeleport.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteVector3D(message.Position);
            buffer.WriteVector2F(message.Rotation);
            buffer.WriteByte(0);
            buffer.WriteVarInt32(message.TeleportId);
        }

        public struct Message : IMessage
        {
            public readonly Vector3 Position;
            public readonly Vector2 Rotation;
            public readonly int TeleportId;

            public Message(Vector3 position, Vector2 rotation, int teleportId)
            {
                Position = position;
                Rotation = rotation;
                TeleportId = teleportId;
            }
        }
    }
}