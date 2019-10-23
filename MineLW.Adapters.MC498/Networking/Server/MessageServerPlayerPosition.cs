using System.Numerics;
using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Server
{
    public class MessageServerPlayerPosition : MessageDeserializer<ClientController, MessageServerPlayerPosition.Message>
    {
        public override IMessage Deserialize(IByteBuffer buffer)
        {
            return new Message(
                buffer.ReadVector3D(),
                buffer.ReadBoolean()
            );
        }

        protected override void Handle(ClientController controller, Message message)
        {
            controller.HandlePlayerPositionUpdate(message.Position);
            controller.HandlePlayerGroundedUpdate(message.Grounded);
        }
        
        public struct Message : IMessage
        {
            public readonly Vector3 Position;
            public readonly bool Grounded;

            public Message(Vector3 position, bool grounded)
            {
                Position = position;
                Grounded = grounded;
            }
        }
    }
}