using System;
using DotNetty.Common.Internal;
using MineLW.API.Blocks;
using MineLW.API.Blocks.Properties;
using MineLW.API.Utils;

namespace MineLW.Blocks
{
    public class BlockState : IBlockState
    {
        public static readonly BlockState Air = new BlockState(
            0,
            new Block(
                0,
                Minecraft.CreateKey("air"),
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

        public override string ToString()
        {
            return Type.ToString() + '[' + string.Join(",", Properties) + ']';
        }

        protected bool Equals(BlockState other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is BlockState other && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public object this[IBlockProperty property] => Properties[PropertyIndex(property)];

        public static bool operator ==(BlockState obj, BlockState other)
        {
            return obj?.Id == other?.Id;
        }

        public static bool operator !=(BlockState obj, BlockState other)
        {
            return obj?.Id != other?.Id;
        }
    }
}