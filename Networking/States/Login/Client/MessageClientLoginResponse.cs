using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking.States.Login.Client
{
    public class MessageClientLoginResponse : MessageSerializer<MessageClientLoginResponse.Message>
    {
        public override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteUtf8(message.Uuid);
            buffer.WriteUtf8(message.Username);
        }
        
        public struct Message : IMessage
        {
            public readonly string Uuid;
            public readonly string Username;

            public Message(string uuid, string username)
            {
                Uuid = uuid;
                Username = username;
            }
        }
    }
}