using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Server
{
    public class MessageServerPlayerTeleportConfirm : MessageDeserializer<ClientController, MessageServerPlayerTeleportConfirm.Message>
    {
        public override IMessage Deserialize(IByteBuffer buffer)
        {
            return new Message(
                buffer.ReadVarInt32()
            );
        }

        protected override void Handle(ClientController controller, Message message)
        {
            controller.HandleTeleportConfirm(message.TeleportId);
        }


        public struct Message : IMessage
        {
            public readonly int TeleportId;

            public Message(int teleportId)
            {
                TeleportId = teleportId;
            }
        }
    }
}