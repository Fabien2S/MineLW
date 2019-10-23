using System.Numerics;
using DotNetty.Buffers;
using MineLW.API.Math;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Server
{
    public class MessageServerPlayerPositionRotation : MessageDeserializer<ClientController, MessageServerPlayerPositionRotation.Message>
    {
        public override IMessage Deserialize(IByteBuffer buffer)
        {
            return new Message(
                buffer.ReadVector3D(),
                buffer.ReadRotation(),
                buffer.ReadBoolean()
            );
        }

        protected override void Handle(ClientController controller, Message message)
        {
            controller.HandlePlayerPositionUpdate(message.Position);
            controller.HandlePlayerRotationUpdate(message.Rotation);
            controller.HandlePlayerGroundedUpdate(message.Grounded);
        }
        
        public struct Message : IMessage
        {
            public readonly Vector3 Position;
            public readonly Rotation Rotation;
            public readonly bool Grounded;

            public Message(Vector3 position, Rotation rotation, bool grounded)
            {
                Position = position;
                Rotation = rotation;
                Grounded = grounded;
            }
        }
    }
}