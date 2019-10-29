using DotNetty.Buffers;
using MineLW.API.Math;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientEntityRotate : MessageSerializer<MessageClientEntityRotate.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteVarInt32(message.Id);
            buffer.WriteRotationB(message.Rotation);
            buffer.WriteBoolean(message.Grounded);
        }
        
        public struct Message : IMessage
        {
            public readonly int Id;
            public readonly Rotation Rotation;
            public readonly bool Grounded;

            public Message(int id, Rotation rotation, bool grounded)
            {
                Id = id;
                Rotation = rotation;
                Grounded = grounded;
            }
        }
        
    }
}