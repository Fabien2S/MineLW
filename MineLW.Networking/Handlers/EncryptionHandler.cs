using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using MineLW.Networking.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace MineLW.Networking.Handlers
{
    public class EncryptionHandler : MessageToMessageCodec<IByteBuffer, IByteBuffer>
    {
        public const string Name = "encryption";

        private readonly IBufferedCipher _encryptCipher;
        private readonly IBufferedCipher _decryptCipher;

        public EncryptionHandler(byte[] sharedSecret)
        {
            _encryptCipher = CreateCipher(sharedSecret, true);
            _decryptCipher = CreateCipher(sharedSecret, false);
        }

        protected override void Encode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            var input = msg.ToArray(out var inputOffset, out var inputLength);
            var outputBuffer = _encryptCipher.ProcessBytes(input, inputOffset, inputLength);
            output.Add(Unpooled.WrappedBuffer(outputBuffer));
        }

        protected override void Decode(IChannelHandlerContext ctx, IByteBuffer msg, List<object> output)
        {
            var input = msg.ToArray(out var inputOffset, out var inputLength);
            var outputBuffer = _decryptCipher.ProcessBytes(input, inputOffset, inputLength);
            output.Add(Unpooled.WrappedBuffer(outputBuffer));
        }

        private static IBufferedCipher CreateCipher(byte[] key, bool encryption)
        {
            var cipher = CipherUtilities.GetCipher("AES/CFB8/NoPadding");
            cipher.Init(encryption, new ParametersWithIV(new KeyParameter(key), key));
            return cipher;
        }
    }
}