using System;
using System.Collections.Generic;

namespace MineLW.API.Blocks
{
    public interface IBlockState : IEquatable<IBlockState>
    {
        int Id { get; }
        IBlock Block { get; }
        IReadOnlyList<dynamic> Properties { get; }
    }
}