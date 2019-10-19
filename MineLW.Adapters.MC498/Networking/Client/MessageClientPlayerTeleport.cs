using System.Numerics;
using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientPlayerTeleport : MessageSerializer<MessageClientPlayerTeleport.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteVector3D(message.Position);
            buffer.WriteRotation(message.Rotation);
            buffer.WriteByte(0);
            buffer.WriteVarInt32(message.TeleportId);
        }

        public struct Message : IMessage
        {
            public readonly Vector3 Position;
            public readonly Quaternion Rotation;
            public readonly int TeleportId;

            public Message(Vector3 position, Quaternion rotation, int teleportId)
            {
                Position = position;
                Rotation = rotation;
                TeleportId = teleportId;
            }
        }
    }
}