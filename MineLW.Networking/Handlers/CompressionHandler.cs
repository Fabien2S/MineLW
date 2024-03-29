using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using ICSharpCode.SharpZipLib.Zip.Compression;
using MineLW.Networking.IO;

namespace MineLW.Networking.Handlers
{
    public class CompressionHandler : MessageToMessageCodec<IByteBuffer, IByteBuffer>
    {
        public const string Name = "compression";

        public int CompressionThreshold { get; set; }

        private readonly byte[] _buffer = new byte[8192];
        private readonly Deflater _deflater;
        private readonly Inflater _inflater;

        public CompressionHandler(int compressionThreshold)
        {
            _deflater= new Deflater(Deflater.DEFAULT_COMPRESSION);
            _inflater= new Inflater();
            
            CompressionThreshold = compressionThreshold;
        }

        protected override void Encode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            var buffer = Unpooled.Buffer();
            
            if (msg.ReadableBytes >= CompressionThreshold)
            {
                var data = msg.ToArray(out var offset, out var count);

                buffer.WriteVarInt32(count);
                
                _deflater.SetInput(data, offset, count);
                _deflater.Finish();
                
                while (!_deflater.IsFinished)
                {
                    var read = _deflater.Deflate(_buffer);
                    buffer.WriteBytes(_buffer, 0, read);
                }
                
                _deflater.Reset();
            }
            else
            {
                buffer.WriteVarInt32(0);
                buffer.WriteBytes(msg);
            }
            
            output.Add(buffer);
        }

        protected override void Decode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            var uncompressedSize = msg.ReadVarInt32();
            if (uncompressedSize == 0)
            {
                var length = msg.ReadableBytes;
                if (length >= CompressionThreshold)
                    throw new DecoderException("Badly compressed message");
                
                msg.Retain();
                output.Add(msg);
                return;
            }
            
            if (uncompressedSize < CompressionThreshold)
                throw new DecoderException("Badly compressed message (uncompressed size: " + uncompressedSize + ')');
            if (uncompressedSize > 1 << 21)
                throw new DecoderException("Badly compressed message (size over protocol limit: " + uncompressedSize + ')');

            var inputBuffer = msg.ToArray(out var offset, out var count);
            _inflater.SetInput(inputBuffer, offset, count);
            
            var outputBuffer = new byte[uncompressedSize];
            var processedBytes = _inflater.Inflate(outputBuffer);
            
            _inflater.Reset();
            
            if(uncompressedSize != processedBytes)
                throw new DecoderException("Badly compressed message");
            
            output.Add(Unpooled.WrappedBuffer(outputBuffer));
        }
    }
}