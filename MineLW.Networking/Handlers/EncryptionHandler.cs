using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using MineLW.Networking.IO;
using Org.BouncyCastle.Crypto;

namespace MineLW.Networking.Handlers
{
    public class EncryptionHandler : MessageToMessageCodec<IByteBuffer, IByteBuffer>
    {
        public const string Name = "encryption";

        private readonly IBufferedCipher _encryptCipher;
        private readonly IBufferedCipher _decryptCipher;

        public EncryptionHandler(byte[] sharedSecret)
        {
            _encryptCipher = Cryptography.CreateCipher(sharedSecret, true);
            _decryptCipher = Cryptography.CreateCipher(sharedSecret, false);
        }

        protected override void Encode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            var inputBuffer = msg.ToArray(out var offset, out var count);
            var outputBuffer = _encryptCipher.ProcessBytes(inputBuffer, offset, count);
            output.Add(Unpooled.WrappedBuffer(outputBuffer));
        }

        protected override void Decode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            var inputBuffer = msg.ToArray(out var offset, out var count);
            var outputBuffer = _decryptCipher.ProcessBytes(inputBuffer, offset, count);
            output.Add(Unpooled.WrappedBuffer(outputBuffer));
        }
    }
}