using System;
using System.Collections.Generic;
using System.Text;
using MineLW.API.Text.Serializers;
using Newtonsoft.Json;

namespace MineLW.API.Text
{
    [JsonConverter(typeof(TextComponentSerializer))]
    public abstract class TextComponent
    {
        public abstract string Id { get; }

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

        public static explicit operator string(TextComponent component)
        {
            var builder = new StringBuilder(component.Value);
            foreach (var child in component.Children)
                builder.Append((string) child);
            return builder.ToString();
        }
    }
}