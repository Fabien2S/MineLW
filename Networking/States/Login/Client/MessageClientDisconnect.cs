using DotNetty.Buffers;
using MineLW.API.Text;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking.States.Login.Client
{
    public class MessageClientDisconnect : MessageSerializer<MessageClientDisconnect.Message>
    {
        public override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteJson(message.Reason);
        }

        public struct Message : IMessage
        {
            public readonly TextComponent Reason;

            public Message(TextComponent reason)
            {
                Reason = reason;
            }
        }
    }
}