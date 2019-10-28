using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using MineLW.Networking.IO;
using MineLW.Networking.Messages;
using NLog;

namespace MineLW.Networking.Handlers
{
    public class MessageEncodingHandler : MessageToMessageCodec<IByteBuffer, IMessage>
    {
        public const string Name = "message_encoding";
        
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly NetworkClient _client;

        public MessageEncodingHandler(NetworkClient client)
        {
            _client = client;
        }

        protected override void Encode(IChannelHandlerContext ctx, IMessage msg, List<object> output)
        {
            var buffer = ctx.Allocator.Buffer();
            
            var state = _client.State;
            var serializer = state.GetSerializer(msg, out var id);
            
            buffer.WriteVarInt32(id);
            serializer.Serialize(buffer, msg);
            
            Logger.Debug("Sending message \"{0}\" (id {1}, {2} bytes) to {3}", msg, id.ToString("X"), buffer.ReadableBytes, _client);

            output.Add(buffer);
        }

        protected override void Decode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            var state = _client.State;
            var id = msg.ReadVarInt32();

            try
            {
                var deserializer = state.GetDeserializer(id);
                if (deserializer == null)
                {
#if DEBUG
                    msg.SkipBytes(msg.ReadableBytes);
                    Logger.Debug("Undefined message {0} in state {1}", id, state);
#endif
                    return;
                }
                
                var message = deserializer.Deserialize(msg);
                if (msg.ReadableBytes > 0)
                    throw new DecoderException("Too many bytes");

                output.Add(message);

                Logger.Debug("Receiving message \"{0}\" (id {1}) from {2}", message, id.ToString("X"), _client);
            }
            catch (DecoderException e)
            {
                throw new DecoderException("Unable to decode message id " + id, e);
            }
        }
    }
}