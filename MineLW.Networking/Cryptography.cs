using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace MineLW.Networking
{
    // Source code from https://github.com/chraft/c-raft
    public static class Cryptography
    {
        public static readonly byte[] PublicKey;

        public static readonly RSACryptoServiceProvider CryptoServiceProvider = new RSACryptoServiceProvider();

        static Cryptography()
        {
            var publicKey = CryptoServiceProvider.ExportParameters(false);
            PublicKey = PublicKeyToAsn1(publicKey);
        }

        /* This is hardcoded since it won't change. 
           It's 1 byte Sequence Tag, 1 byte Sequence Length, 1 byte Oid Tag, 1 byte Oid Length, 9 bytes Oid, 1 byte Null Tag, 1 byte Null */
        private static readonly byte[] AlgorithmId =
        {
            0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00
        };

        private static byte[] PublicKeyToAsn1(RSAParameters parameters)
        {
            // Oid - Tag: 0x06 - Length: 0x09 - Octets: 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01
            // AlgorithmId - Tag: 0x30 - Length: 0x0D - Octets: 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00

            // mod - Tag: 0x02 - Length: ? - Octets: 0x00?, parameters.Modulus
            // exp - Tag: 0x02 - Length: ? - Octets: 0x00?, parameters.Exponent            
            var mod = CreateIntegerPos(parameters.Modulus);
            var exp = CreateIntegerPos(parameters.Exponent);

            // Sequence(mod + exp) - Tag: 0x30 - Length: ? - Octets: mod + exp
            // Key - Tag: 0x03 - Length: ? - Octets: 0x00, Sequence(mod + exp)
            // PublicKey - Tag: 0x30 - Length: ? - Octets: AlgorithmId + Key
            // AsnMessage - PublicKey

            var sequenceOctetsLength = mod.Length + exp.Length;
            var sequenceLengthArray = LengthToByteArray(sequenceOctetsLength);

            var keyOctetsLength = sequenceLengthArray.Length + sequenceOctetsLength + 2;
            var keyLengthArray = LengthToByteArray(keyOctetsLength);

            var publicKeyOctetsLength = keyOctetsLength + keyLengthArray.Length + AlgorithmId.Length + 1;
            var publicKeyLengthArray = LengthToByteArray(publicKeyOctetsLength);

            var messageLength = publicKeyOctetsLength + publicKeyLengthArray.Length + 1;

            var message = new byte[messageLength];
            var index = 0;

            message[index++] = 0x30;

            Buffer.BlockCopy(publicKeyLengthArray, 0, message, index, publicKeyLengthArray.Length);
            index += publicKeyLengthArray.Length;

            Buffer.BlockCopy(AlgorithmId, 0, message, index, AlgorithmId.Length);

            index += AlgorithmId.Length;

            message[index++] = 0x03;

            Buffer.BlockCopy(keyLengthArray, 0, message, index, keyLengthArray.Length);
            index += keyLengthArray.Length;

            message[index++] = 0x00;
            message[index++] = 0x30;

            Buffer.BlockCopy(sequenceLengthArray, 0, message, index, sequenceLengthArray.Length);
            index += sequenceLengthArray.Length;

            Buffer.BlockCopy(mod, 0, message, index, mod.Length);
            index += mod.Length;
            Buffer.BlockCopy(exp, 0, message, index, exp.Length);

            //Console.WriteLine(BitConverter.ToString(message));
            return message;
        }

        private static byte[] LengthToByteArray(int octetsLength)
        {
            byte[] length;

            // Length: 0 <= l < 0x80
            if (octetsLength < 0x80)
            {
                length = new byte[1];
                length[0] = (byte) octetsLength;
            }
            // 0x80 < length <= 0xFF
            else if (octetsLength <= 0xFF)
            {
                length = new byte[2];
                length[0] = 0x81;
                length[1] = (byte) (octetsLength & 0xFF);
            }

            //
            // We should almost never see these...
            //

            // 0xFF < length <= 0xFFFF
            else if (octetsLength <= 0xFFFF)
            {
                length = new byte[3];
                length[0] = 0x82;
                length[1] = (byte) ((octetsLength & 0xFF00) >> 8);
                length[2] = (byte) (octetsLength & 0xFF);
            }

            // 0xFFFF < length <= 0xFFFFFF
            else if (octetsLength <= 0xFFFFFF)
            {
                length = new byte[4];
                length[0] = 0x83;
                length[1] = (byte) ((octetsLength & 0xFF0000) >> 16);
                length[2] = (byte) ((octetsLength & 0xFF00) >> 8);
                length[3] = (byte) (octetsLength & 0xFF);
            }
            // 0xFFFFFF < length <= 0xFFFFFFFF
            else
            {
                length = new byte[5];
                length[0] = 0x84;
                length[1] = (byte) ((octetsLength & 0xFF000000) >> 24);
                length[2] = (byte) ((octetsLength & 0xFF0000) >> 16);
                length[3] = (byte) ((octetsLength & 0xFF00) >> 8);
                length[4] = (byte) (octetsLength & 0xFF);
            }

            return length;
        }

        private static byte[] CreateIntegerPos(byte[] value)
        {
            byte[] newInt;

            if (value[0] > 0x7F)
            {
                // Integer length + Positive byte
                var length = LengthToByteArray(value.Length + 1);
                var index = 1;
                // Tag + Length + Positive byte + Value
                newInt = new byte[value.Length + 2 + length.Length];
                // Int Tag
                newInt[0] = 0x02;
                // Int Length
                foreach (var t in length)
                    newInt[index++] = t;

                // Makes the number positive
                newInt[index++] = 0x00;
                Buffer.BlockCopy(value, 0, newInt, index, value.Length);
            }
            else
            {
                var length = LengthToByteArray(value.Length);
                var index = 1;

                // Tag + Length + Value
                newInt = new byte[value.Length + 1 + length.Length];
                // Int Tag
                newInt[0] = 0x02;
                // Int Length
                foreach (var t in length)
                    newInt[index++] = t;

                Buffer.BlockCopy(value, 0, newInt, index, value.Length);
            }

            return newInt;
        }

        public static string GetServerHash(byte[] serverId, byte[] secretKey, byte[] publicKey)
        {
            using var sha = new SHA1CryptoServiceProvider();
            sha.TransformBlock(serverId, 0, serverId.Length, serverId, 0);
            sha.TransformBlock(secretKey, 0, secretKey.Length, secretKey, 0);
            sha.TransformBlock(publicKey, 0, publicKey.Length, publicKey, 0);
            sha.TransformFinalBlock(new byte[0], 0, 0);
                
            var hash = sha.Hash;
            var negative = (hash[0] & 0x80) == 0x80;
                
            if (negative)
            {
                int i;
                var carry = true;
                for (i = hash.Length - 1; i >= 0; i--)
                {
                    hash[i] = (byte) ~hash[i];
                    if (!carry)
                        continue;
                    carry = hash[i] == 0xFF;
                    hash[i]++;
                }
            }

            var result = hash
                .Aggregate(string.Empty, (current, t) => current + t.ToString("x2", CultureInfo.InvariantCulture))
                .TrimStart('0');
                
            if (negative)
                result = "-" + result;
                
            return result;
        }

        public static IBufferedCipher CreateCipher(byte[] key, bool encryption)
        {
            var cipher = CipherUtilities.GetCipher("AES/CFB8/NoPadding");
            cipher.Init(encryption, new ParametersWithIV(new KeyParameter(key), key));
            return cipher;
        }
    }
}