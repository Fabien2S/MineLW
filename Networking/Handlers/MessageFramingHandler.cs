using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using MineLW.Networking.IO;

namespace MineLW.Networking.Handlers
{
    public class MessageFramingHandler : MessageToMessageCodec<IByteBuffer, IByteBuffer>
    {
        public const string Name = "message_framing";

        protected override void Encode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            var buffer = ctx.Allocator.Buffer();
            buffer.WriteVarInt32(msg.ReadableBytes);
            buffer.WriteBytes(msg);
        }

        protected override void Decode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            msg.MarkReaderIndex();

            if (!msg.TryReadVarInt32(out var length) || msg.ReadableBytes < length)
            {
                msg.ResetReaderIndex();
                return;
            }

            var buffer = msg.ReadBytes(length);
            output.Add(buffer);
        }
    }
}