using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Ionic.Zlib;
using MineLW.Networking.IO;

namespace MineLW.Networking.Handlers
{
    public class CompressionHandler : MessageToMessageCodec<IByteBuffer, IByteBuffer>
    {
        public const string Name = "compression";
        
        private readonly ZlibCodec _compressCodec;
        private readonly ZlibCodec _decompressCodec;

        private readonly int _compressionThreshold;

        public CompressionHandler(int compressionThreshold)
        {
            _compressCodec= new ZlibCodec(CompressionMode.Compress);
            _decompressCodec= new ZlibCodec(CompressionMode.Decompress);
            
            _compressionThreshold = compressionThreshold;
        }

        protected override void Encode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            var data = msg.ToArray(out var offset, out var length);
            var buffer = ctx.Allocator.Buffer(length);

            if (msg.ReadableBytes >= _compressionThreshold)
            {
                _compressCodec.InputBuffer = data;
                _compressCodec.NextIn = offset;
                _compressCodec.AvailableBytesIn = length;

                var compressedData = new byte[length];
                _compressCodec.OutputBuffer = compressedData;
                _compressCodec.NextOut = 0;
                _compressCodec.AvailableBytesOut = length;
                
                _compressCodec.Deflate(FlushType.Finish);
                _compressCodec.EndDeflate();
                _compressCodec.ResetDeflate();
                
                buffer.WriteVarInt32(length);
                buffer.WriteBytes(compressedData);
            }
            else
            {
                buffer.WriteVarInt32(0);
                buffer.WriteBytes(data, offset, length);
            }

            output.Add(buffer);
        }

        protected override void Decode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            var uncompressedSize = msg.ReadVarInt32();
            if (uncompressedSize == 0)
            {
                var length = msg.ReadableBytes;
                if (length >= _compressionThreshold)
                    throw new DecoderException("Badly compressed message");

                var buffer = ctx.Allocator.Buffer(length);
                buffer.ReadBytes(msg, length);
                output.Add(buffer);
            }
            else
            {
                var compressedData = msg.ToArray(out var offset, out var length);
                
                _decompressCodec.InputBuffer = compressedData;
                _decompressCodec.NextIn = offset;
                _decompressCodec.AvailableBytesIn = length;

                var data = new byte[uncompressedSize];
                _decompressCodec.OutputBuffer = data;
                _decompressCodec.NextOut = 0;
                _decompressCodec.AvailableBytesOut = uncompressedSize;
                
                _decompressCodec.Inflate(FlushType.Finish);
                _decompressCodec.EndInflate();
                
                output.Add(Unpooled.WrappedBuffer(data));
            }
        }
    }
}