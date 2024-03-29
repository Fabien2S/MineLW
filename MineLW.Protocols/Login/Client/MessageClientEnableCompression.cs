using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using MineLW.Networking.Messages.Serialization;

namespace MineLW.Protocols.Login.Client
{
    public class MessageClientEnableCompression : MessageSerializer<MessageClientEnableCompression.Message>
    {
        protected override void Serialize(IByteBuffer buffer, Message message)
        {
            buffer.WriteVarInt32(message.Threshold);
        }
        
        public struct Message : IMessage
        {
            public readonly int Threshold;

            public Message(int threshold)
            {
                Threshold = threshold;
            }
        }
    }
}