using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MineLW.API.Text.Serializers;

namespace MineLW.API.Text
{
    [JsonConverter(typeof(TextComponentSerializer))]
    public abstract class TextComponent : IEnumerable<TextComponent>
    {
        public TextComponent Parent { get; private set; }
        public abstract string Id { get; }

        public string Value { get; protected set; }
        
        public bool HasColor { get; private set; }
        public TextColor Color { get; private set; } = TextColor.White;
        
        public bool HasStyle { get; private set; }
        public TextStyles Style { get; private set; } = TextStyles.None;
        
        public int ChildCount => _children.Count;

        private readonly List<TextComponent> _children = new List<TextComponent>();

        public TextComponent WithValue(string value)
        {
            Value = value;
            return this;
        }

        public TextComponent WithColor(TextColor color)
        {
            HasColor = true;
            Color = color;
            return this;
        }

        public TextComponent WithStyle(TextStyles style)
        {
            HasStyle = true;
            Style = style;
            return this;
        }

        public T Append<T>() where T : TextComponent, new()
        {
            var child = new T
            {
                Parent = this
            };
            _children.Add(child);
            return child;
        }

        public IEnumerator<TextComponent> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}