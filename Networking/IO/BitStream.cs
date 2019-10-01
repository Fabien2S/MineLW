using System;
using System.IO;
using System.Net;
using System.Text;

namespace MineLW.Networking.IO
{
    public class BitStream : Stream
    {
        private const byte BufferSize = 16;

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;

        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        private readonly Stream _stream;
        private readonly byte[] _buffer; // buffer used by primitive

        public BitStream(Stream stream)
        {
            _stream = stream;
            _buffer = new byte[BufferSize];
        }

        private void FillBuffer(int numBytes)
        {
            if (numBytes < 0 || numBytes > BufferSize)
                throw new ArgumentOutOfRangeException(nameof(numBytes),
                    "Buffer too small (" + numBytes + " < " + BufferSize);

            int read;
            if (numBytes == 1)
            {
                read = _stream.ReadByte();
                if (read == -1)
                    throw new EndOfStreamException();
                _buffer[0] = (byte) read;
            }
            else
            {
                var bytesRead = 0;
                do
                {
                    read = _stream.Read(_buffer, bytesRead, numBytes - bytesRead);
                    if (read == 0)
                        throw new EndOfStreamException();
                    bytesRead += read;
                } while (bytesRead < numBytes);
            }
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long length)
        {
            _stream.SetLength(length);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        public byte[] ReadBytes(int numBytes)
        {
            var result = new byte[numBytes];

            var numRead = 0;
            do
            {
                var n = _stream.Read(result, numRead, numBytes);
                if (n == 0)
                    break;
                numRead += n;
                numBytes -= n;
            } while (numBytes > 0);

            if (numRead == result.Length)
                return result;

            // Trim array.  This should happen on EOF & possibly net streams.
            var copy = new byte[numRead];
            Buffer.BlockCopy(result, 0, copy, 0, numRead);
            result = copy;

            return result;
        }

        public short ReadInt16()
        {
            FillBuffer(2);
            var bigEndianValue = (short) (_buffer[0] | _buffer[1] << 8);
            return IPAddress.NetworkToHostOrder(bigEndianValue);
        }

        public void WriteInt16(short value)
        {
            var i = IPAddress.HostToNetworkOrder(value);
            _buffer[0] = (byte) i;
            _buffer[1] = (byte) (i >> 8);
            Write(_buffer, 0, 2);
        }

        public int ReadInt32()
        {
            FillBuffer(4);
            var bigEndianValue = _buffer[0] | _buffer[1] << 8 | _buffer[2] << 16 | _buffer[3] << 24;
            return IPAddress.NetworkToHostOrder(bigEndianValue);
        }

        public void WriteInt32(int value)
        {
            var bigEndianValue = IPAddress.HostToNetworkOrder(value);
            _buffer[0] = (byte) bigEndianValue;
            _buffer[1] = (byte) (bigEndianValue >> 8);
            _buffer[2] = (byte) (bigEndianValue >> 16);
            _buffer[3] = (byte) (bigEndianValue >> 24);
            Write(_buffer, 0, 4);
        }

        public long ReadInt64()
        {
            FillBuffer(8);
            var lo = (uint) (_buffer[0] | _buffer[1] << 8 | _buffer[2] << 16 | _buffer[3] << 24);
            var hi = (uint) (_buffer[4] | _buffer[5] << 8 | _buffer[6] << 16 | _buffer[7] << 24);
            return IPAddress.NetworkToHostOrder((long) hi << 32 | lo);
        }

        public void WriteInt64(long value)
        {
            var bigEndianValue = IPAddress.HostToNetworkOrder(value);
            _buffer[0] = (byte) bigEndianValue;
            _buffer[1] = (byte) (bigEndianValue >> 8);
            _buffer[2] = (byte) (bigEndianValue >> 16);
            _buffer[3] = (byte) (bigEndianValue >> 24);
            _buffer[4] = (byte) (bigEndianValue >> 32);
            _buffer[5] = (byte) (bigEndianValue >> 40);
            _buffer[6] = (byte) (bigEndianValue >> 48);
            _buffer[7] = (byte) (bigEndianValue >> 56);
            Write(_buffer, 0, 8);
        }

        public string ReadString(short maxLen = short.MaxValue)
        {
            var len = _stream.ReadInt32();
            if (len > maxLen)
                throw new IndexOutOfRangeException("String is too long (" + len + " > " + maxLen + ")");
            var bytes = ReadBytes(len);
            return Encoding.UTF8.GetString(bytes);
        }

        public void WriteString(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            var len = bytes.Length;
            if (len > short.MaxValue)
                throw new ArgumentException("String is too long (" + len + " > " + short.MaxValue + ")");

            _stream.WriteInt32(len);
            Write(bytes);
        }
    }
}