using System;
using DotNetty.Buffers;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Server
{
    public class MessageServerPingResponse : MessageDeserializer<ClientController, MessageServerPingResponse.Message>
    {
        public override IMessage Deserialize(IByteBuffer buffer)
        {
            return new Message(
                buffer.ReadLong()
            );
        }

        protected override void Handle(ClientController controller, Message message)
        {
            controller.HandlePingResponse(message.PingId);
        }

        public struct Message : IMessage
        {
            public readonly long PingId;

            public Message(long pingId)
            {
                PingId = pingId;
            }
        }
    }
}