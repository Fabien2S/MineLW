﻿using System;
using System.Collections.Generic;
using MineLW.API.Blocks.Properties;
using MineLW.API.Utils;

namespace MineLW.API.Blocks
{
    public interface IBlock : IEquatable<IBlock>
    {
        int Id { get; }
        Identifier Name { get; }
        IReadOnlyList<IBlockProperty> Properties { get; }
        IReadOnlyList<dynamic> DefaultValues { get; }
    }
}