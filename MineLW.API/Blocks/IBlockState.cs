using System;

namespace MineLW.API.Blocks
{
    public interface IBlockState : IEquatable<IBlockState>
    {
        int Id { get; }
        IBlock Type { get; }
        object[] Properties { get; }
    }
}