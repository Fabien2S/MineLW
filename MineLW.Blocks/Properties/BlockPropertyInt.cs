using System;
using System.Globalization;
using System.Linq;

namespace MineLW.Blocks.Properties
{
    public class BlockPropertyInt : BlockProperty<int>
    {
        private readonly int _min;
        private readonly int _max;

        public BlockPropertyInt(string name, int min, int max) : base(name, CreateRangeCollection(min, max))
        {
            _min = min;
            _max = max;
        }

        public BlockPropertyInt(string name, int[] values) : base(name, values)
        {
            _min = values.Min();
            _max = values.Max();
        }

        public override object Parse(string source)
        {
            if (!int.TryParse(source, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            {
                throw new ArgumentException(
                    "source isn't a valid int",
                    nameof(source)
                );
            }

            if (value < _min || value > _max)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(source),
                    value,
                    "source is either greater or less than the specified range"
                );
            }

            return value;
        }

        private static int[] CreateRangeCollection(int min, int max)
        {
            var possibilities = new int[max - min + 1];
            for (var i = min; i <= max; i++)
                possibilities[i - min] = i;
            return possibilities;
        }
    }
}