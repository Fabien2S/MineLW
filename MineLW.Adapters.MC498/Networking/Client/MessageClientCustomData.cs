using DotNetty.Buffers;
using MineLW.API.Utils;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Adapters.MC498.Networking.Client
{
    public class MessageClientCustomData : MessageSerializer<MessageClientCustomData.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteIdentifier(message.Channel);
            buffer.WriteBytes(message.Buffer);
        }

        public struct Message : IMessage
        {
            public readonly Identifier Channel;
            public readonly byte[] Buffer;

            public Message(Identifier channel, byte[] buffer)
            {
                Channel = channel;
                Buffer = buffer;
            }
        }
    }
}