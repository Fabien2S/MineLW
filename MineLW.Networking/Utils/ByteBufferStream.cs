using System;
using System.IO;
using DotNetty.Buffers;
using DotNetty.Common.Utilities;

namespace MineLW.Networking.Utils
{
    public class ByteBufferStream : Stream
    {
        public override bool CanRead => _mode == Mode.Read;
        public override bool CanWrite => _mode == Mode.Write;
        public override bool CanSeek => true;

        public override long Length => _mode == Mode.Read ? _buffer.WriterIndex : _buffer.ReaderIndex;

        public override long Position
        {
            get => _mode == Mode.Read ? _buffer.ReaderIndex : _buffer.WriterIndex;
            set
            {
                if (_mode == Mode.Read)
                    _buffer.SetReaderIndex((int) value);
                else
                    _buffer.SetWriterIndex((int) value);
            }
        }

        private readonly IByteBuffer _buffer;
        private readonly Mode _mode;

        private bool _releaseReferenceOnClosure;

        public ByteBufferStream(IByteBuffer buffer, Mode mode, bool releaseReferenceOnClosure = false)
        {
            _buffer = buffer;
            _mode = mode;
            _releaseReferenceOnClosure = releaseReferenceOnClosure;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_releaseReferenceOnClosure)
                return;

            _releaseReferenceOnClosure = false;
            if (disposing)
                _buffer.Release();
            else
                _buffer.SafeRelease();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_mode != Mode.Read)
                throw new NotSupportedException();
            if (offset + count > buffer.Length)
                throw new ArgumentException("The sum of offset and count is larger than the output length");

            var length = Math.Min(count, _buffer.ReadableBytes);
            _buffer.ReadBytes(buffer, offset, length);
            return length;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.End:
                    Position = offset + Length;
                    break;
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }

            return Position;
        }

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_mode != Mode.Write)
                throw new NotSupportedException();
            _buffer.WriteBytes(buffer, offset, count);
        }

        public enum Mode
        {
            Write,
            Read
        }
    }
}