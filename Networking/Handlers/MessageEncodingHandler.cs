using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using MineLW.Debugging;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;

namespace MineLW.Networking.Handlers
{
    public class MessageEncodingHandler : MessageToMessageCodec<IByteBuffer, object>
    {
        public const string Name = "message_encoding";
        
        private static readonly Logger Logger = LogManager.GetLogger<MessageEncodingHandler>();

        private readonly NetworkClient _client;

        public MessageEncodingHandler(NetworkClient client)
        {
            _client = client;
        }

        protected override void Encode(IChannelHandlerContext ctx, object msg, List<object> output)
        {
            foreach (var message in output)
            {
                var buffer = ctx.Allocator.Buffer();
                MessageManager.Serialize(buffer, message);
                
                Logger.Debug("Sending message \"{0}\" to {1}", message, _client);

                output.Add(buffer);
            }
        }

        protected override void Decode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            var id = msg.ReadVarInt32();
            Logger.Debug("Message {0} received from {1}", id, _client);
        }
    }
}