using DotNetty.Buffers;
using MineLW.API.Text;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Client.MC498.Client
{
    public class MessageClientChatMessage : MessageSerializer<MessageClientChatMessage.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteJson(message.ChatMessage);
        }
        
        public struct Message : IMessage
        {
            public readonly TextComponent ChatMessage;

            public Message(TextComponent chatMessage)
            {
                ChatMessage = chatMessage;
            }
        }
    }
}