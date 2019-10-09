using System;
using System.Collections.ObjectModel;

namespace MineLW.Blocks.Properties
{
    public class BlockPropertyBool : BlockProperty<bool>
    {
        private static readonly ReadOnlyCollection<bool> Boolean = Array.AsReadOnly(new[] {true, false});

        public BlockPropertyBool(string name) : base(name, Boolean)
        {
        }

        public override object Parse(string source)
        {
            return source.Equals("true", StringComparison.Ordinal);
        }
    }
}