/*
 * Generated class. Do not edit manually.
 */

using System.Collections.Generic;
using System.Collections.Immutable;
using MineLW.API;
using MineLW.API.Registries;

namespace MineLW.Adapters.MC498.Blocks
{
    public static class BlockTypes
    {
        public static readonly IReadOnlyList<dynamic> NoValues = ImmutableArray<dynamic>.Empty;
        
        public static void Register(IBlockRegistry registry)
        {
            registry.Register(Minecraft.Blocks.Air, BlockProperties.NoProperties, NoValues);
            registry.Register(Minecraft.Blocks.Stone, BlockProperties.NoProperties, NoValues);
            registry.Register(Minecraft.Blocks.Granite, BlockProperties.NoProperties, NoValues);
        }
    }
}