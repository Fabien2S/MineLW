using DotNetty.Buffers;

namespace MineLW.Serialization.NBT
{
    public class NbtLongArray : NbtTag<long[]>
    {
        public override byte Id { get; } = 12;

        public NbtLongArray(long[] value = null) : base(value ?? new long[0])
        {
        }

        public override void Serialize(IByteBuffer buffer)
        {
            buffer.WriteInt(Value.Length);
            foreach (var l in Value)
                buffer.WriteLong(l);
        }
    }
}