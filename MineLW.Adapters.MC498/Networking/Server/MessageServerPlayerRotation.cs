using DotNetty.Buffers;
using MineLW.API.Math;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Server
{
    public class MessageServerPlayerRotation : MessageDeserializer<ClientController, MessageServerPlayerRotation.Message>
    {
        public override IMessage Deserialize(IByteBuffer buffer)
        {
            return new Message(
                buffer.ReadRotation(),
                buffer.ReadBoolean()
            );
        }

        protected override void Handle(ClientController controller, Message message)
        {
            controller.HandlePlayerGroundedUpdate(message.Grounded);
            controller.HandlePlayerRotationUpdate(message.Position);
        }
        
        public struct Message : IMessage
        {
            public readonly Rotation Position;
            public readonly bool Grounded;

            public Message(Rotation position, bool grounded)
            {
                Position = position;
                Grounded = grounded;
            }
        }
    }
}