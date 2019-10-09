using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Protocols.Login.Client
{
    public class MessageClientEncryptionRequest : MessageSerializer<MessageClientEncryptionRequest.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteUtf8(message.ServerId);
            buffer.WriteByteArray(message.PublicKey);
            buffer.WriteByteArray(message.Signature);
        }

        public struct Message : IMessage
        {
            public readonly string ServerId;
            public readonly byte[] PublicKey;
            public readonly byte[] Signature;

            public Message(string serverId, byte[] publicKey, byte[] signature)
            {
                ServerId = serverId;
                PublicKey = publicKey;
                Signature = signature;
            }
        }
    }
}