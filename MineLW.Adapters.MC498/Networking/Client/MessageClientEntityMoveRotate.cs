using DotNetty.Buffers;
using MineLW.API.Math;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientEntityMoveRotate : MessageSerializer<MessageClientEntityMoveRotate.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteVarInt32(message.Id);
            buffer.WriteShort(message.DeltaX);
            buffer.WriteShort(message.DeltaY);
            buffer.WriteShort(message.DeltaZ);
            buffer.WriteRotationF(message.Rotation);
            buffer.WriteBoolean(message.Grounded);
        }
        
        public struct Message : IMessage
        {
            public readonly int Id;
            public readonly short DeltaX;
            public readonly short DeltaY;
            public readonly short DeltaZ;
            public readonly Rotation Rotation;
            public readonly bool Grounded;

            public Message(int id, short deltaX, short deltaY, short deltaZ, Rotation rotation, bool grounded)
            {
                Id = id;
                DeltaX = deltaX;
                DeltaY = deltaY;
                DeltaZ = deltaZ;
                Rotation = rotation;
                Grounded = grounded;
            }
        }
        
    }
}