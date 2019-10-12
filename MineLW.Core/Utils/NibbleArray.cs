using System.Linq;
using DotNetty.Buffers;
using MineLW.Networking.IO;
using MineLW.Networking.Serialization;

namespace MineLW.Utils
{
    public class NibbleArray : INetworkSerializer
    {
        private const int MaxLowestValue = 15;
        private const int MaxHighestValue = MaxLowestValue * MaxLowestValue + MaxLowestValue;

        public bool Empty
        {
            get
            {
                if (!_dirty)
                    return _empty;

                _empty = _buffer.All(b => b == 0);
                _dirty = false;
                return _empty;
            }
        }

        private readonly byte[] _buffer;

        private bool _empty = true;
        private bool _dirty;

        public NibbleArray(int length)
        {
            _buffer = new byte[length];
        }

        public byte this[int index]
        {
            get
            {
                var arrayIndex = ArrayIndex(index);
                var b = _buffer[arrayIndex];
                return (byte) (IsLowestBits(index) ? b & MaxLowestValue : (b >> 4) & MaxLowestValue);
            }
            set
            {
                var arrayIndex = ArrayIndex(index);
                if (IsLowestBits(index))
                    _buffer[arrayIndex] = (byte) (_buffer[arrayIndex] & MaxHighestValue | value & MaxLowestValue);
                else
                    _buffer[arrayIndex] = (byte) (_buffer[arrayIndex] & MaxLowestValue | (value & MaxLowestValue) << 4);

                _dirty = true;
            }
        }

        public void Serialize(IByteBuffer buffer)
        {
            buffer.WriteByteArray(_buffer);
        }

        private static int ArrayIndex(int nibbleIndex)
        {
            return nibbleIndex / 2;
        }

        private static bool IsLowestBits(int nibbleIndex)
        {
            return (nibbleIndex & 1) == 0;
        }
    }
}