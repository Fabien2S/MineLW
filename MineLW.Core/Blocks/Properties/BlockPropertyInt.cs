using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MineLW.Blocks.Properties
{
    public class BlockPropertyInt : BlockProperty<int>
    {
        private readonly int _min;
        private readonly int _max;

        public BlockPropertyInt(string name, int min, int max) : base(name, SetFromRange(min, max))
        {
            _min = min;
            _max = max;
        }

        public BlockPropertyInt(string name, int[] values) : base(name, Array.AsReadOnly(values))
        {
            _min = values.Min();
            _max = values.Max();
        }

        public override object Parse(string source)
        {
            if (!int.TryParse(source, out var value))
                throw new ArgumentException("Invalid int property value", nameof(source));
            if(value < _min || value > _max)
                throw new ArgumentOutOfRangeException(nameof(source), "Property value out of range (value: " + value + ", range: [" + _min + ',' + _max + "])");

            return value;
        }

        private static ReadOnlyCollection<int> SetFromRange(int min, int max)
        {
            var possibilities = new int[max - min + 1];
            for (var i = min; i <= max; i++)
                possibilities[i - min] = i;
            return Array.AsReadOnly(possibilities);
        }
    }
}