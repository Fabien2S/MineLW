using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MineLW.API.Text.Serializers;
using Newtonsoft.Json;

namespace MineLW.API.Text
{
    [JsonConverter(typeof(TextComponentSerializer))]
    public abstract class TextComponent
    {
        public readonly string Value;
        public readonly List<TextComponent> Children;

        public bool HasColor { get; private set; }

        public TextColor Color
        {
            get => _color;
            set
            {
                HasColor = true;
                _color = value;
            }
        }

        public bool HasStyle { get; private set; }

        public TextStyles Style
        {
            get => _style;
            set
            {
                HasStyle = true;
                _style = value;
            }
        }

        private TextColor _color;
        private TextStyles _style;

        protected TextComponent(string value = null)
        {
            Value = value ?? string.Empty;
            Children = new List<TextComponent>();
        }

        private bool Equals(TextComponent other)
        {
            return
                Value.Equals(other.Value, StringComparison.Ordinal) &&
                HasColor == other.HasColor &&
                _color == other._color &&
                HasStyle == other.HasStyle &&
                _style == other._style &&
                Children.SequenceEqual(other.Children);
        }

        public override bool Equals(object obj)
        {
            return obj is TextComponent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder
                .Append(GetType().Name)
                .Append('{')
                .Append("value=\"")
                .Append(Value)
                .Append('"');

            if (HasColor)
                builder
                    .Append(",color=\"")
                    .Append(_color)
                    .Append('"');

            if (HasStyle)
            {
                var styleName = Enum.GetName(typeof(TextStyles), _style);
                builder
                    .Append(",style=\"")
                    .Append(styleName)
                    .Append('"');
            }

            if (Children.Count > 0)
            {
                builder.Append(",extra=");
                foreach (var textComponent in Children)
                    builder.Append(textComponent);
            }

            builder.Append('}');

            return builder.ToString();
        }
    }
}