using System;
using DotNetty.Common.Internal;
using MineLW.API;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Properties;

namespace MineLW.Blocks
{
    public class BlockState : IBlockState
    {
        public static readonly IBlockState Air = new BlockState(
            0,
            new Block(
                0,
                Minecraft.CreateIdentifier("air"),
                new IBlockProperty[0],
                EmptyArrays.EmptyObjects
            )
        );

        public int Id { get; }
        public IBlock Type { get; }
        public object[] Properties { get; }

        public BlockState(int id, Block type, object[] properties = null)
        {
            Id = id;
            Type = type;
            Properties = properties ?? type.DefaultValues;
        }

        private int PropertyIndex(IBlockProperty property)
        {
            var index = Type.Properties.IndexOf(property);
            if (index == -1)
                throw new ArgumentException("The block \"" + Type + "\" doesn't support the property " + property);
            return index;
        }

        public bool Equals(IBlockState other)
        {
            return Id == other?.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is IBlockState other && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return Type.ToString() + '[' + string.Join(",", Properties) + ']';
        }

        public object this[IBlockProperty property] => Properties[PropertyIndex(property)];
    }
}