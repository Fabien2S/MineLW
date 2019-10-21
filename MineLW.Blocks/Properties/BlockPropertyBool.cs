using System;

namespace MineLW.Blocks.Properties
{
    public class BlockPropertyBool : BlockProperty<bool>
    {
        private static readonly bool[] AllValues = {true, false};

        public BlockPropertyBool(string name) : base(name, AllValues)
        {
        }

        public override object Parse(string source)
        {
            if (source.Equals("true", StringComparison.Ordinal))
                return true;
            if (source.Equals("false", StringComparison.Ordinal))
                return false;
            throw new ArgumentException("source is neither true or false", nameof(source));
        }
    }
}