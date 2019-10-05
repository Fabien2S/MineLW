using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Networking.States.Login.Server
{
    public class MessageServerLoginRequest : MessageDeserializer<LoginController, MessageServerLoginRequest.Message>
    {
        public override IMessage Deserialize(IByteBuffer buffer)
        {
            return new Message(
                buffer.ReadUtf8()
            );
        }

        public override void Handle(LoginController controller, Message message)
        {
            controller.HandleLoginRequest(message.Username);
        }

        public struct Message : IMessage
        {
            public readonly string Username;

            public Message(string username)
            {
                Username = username;
            }
        }
    }
}