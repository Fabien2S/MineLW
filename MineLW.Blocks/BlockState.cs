using System.Collections.Generic;
using MineLW.API.Blocks;

namespace MineLW.Blocks
{
    public class BlockState : IBlockState
    {
        public int Id { get; }
        public IBlock Block { get; }
        public IReadOnlyList<dynamic> Properties { get; }

        public BlockState(int id, IBlock type, IReadOnlyList<dynamic> properties)
        {
            Id = id;
            Block = type;
            Properties = properties;
        }

        public bool Equals(IBlockState other)
        {
            return Id == other?.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is IBlockState other && other.Id == Id;
        }

        public override int GetHashCode() => Id;
        public override string ToString() => Block.ToString() + '[' + string.Join(",", Properties) + ']';
    }
}