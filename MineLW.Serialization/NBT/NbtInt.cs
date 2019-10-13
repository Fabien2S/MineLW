using DotNetty.Buffers;

namespace MineLW.Serialization.NBT
{
    public class NbtInt : NbtTag<int>
    {
        public override byte Id { get; } = 3;

        public NbtInt(int value = 0) : base(value)
        {
        }
        
        public override void Serialize(IByteBuffer buffer)
        {
            buffer.WriteInt(Value);
        }
    }
}