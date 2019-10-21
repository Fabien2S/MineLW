using MineLW.API.Blocks;

namespace MineLW.Blocks
{
    public class BlockState : IBlockState
    {
        public int Id { get; }
        public IBlock Type { get; }
        public object[] Properties { get; }

        public BlockState(int id, IBlock type, object[] properties)
        {
            Id = id;
            Type = type;
            Properties = properties;
        }

        /*private int PropertyIndex(IBlockProperty property)
        {
            var index = Type.Properties.IndexOf(property);
            if (index == -1)
                throw new ArgumentException("The block \"" + Type + "\" doesn't support the property " + property);
            return index;
        }*/

        public bool Equals(IBlockState other)
        {
            return Id == other?.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is IBlockState other && other.Id == Id;
        }

        public override int GetHashCode() => Id;
        public override string ToString() => Type.ToString() + '[' + string.Join(",", Properties) + ']';
    }
}