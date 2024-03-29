using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Protocols.Login.Server
{
    public class
        MessageServerEncryptionResponse : MessageDeserializer<LoginController, MessageServerEncryptionResponse.Message>
    {
        public override IMessage Deserialize(IByteBuffer buffer)
        {
            return new Message(
                buffer.ReadByteArray(),
                buffer.ReadByteArray()
            );
        }

        protected override void Handle(LoginController controller, Message message)
        {
            controller.HandleEncryptionResponse(message.SharedSecret, message.Signature);
        }

        public struct Message : IMessage
        {
            public readonly byte[] SharedSecret;
            public readonly byte[] Signature;

            public Message(byte[] sharedSecret, byte[] signature)
            {
                SharedSecret = sharedSecret;
                Signature = signature;
            }
        }
    }
}