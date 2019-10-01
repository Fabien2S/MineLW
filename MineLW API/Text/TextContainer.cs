﻿using System.Collections;
using System.Collections.Generic;

namespace MineLW.API.Text
{
    public class TextContainer : ITextComponent, IEnumerable<ITextComponent>
    {
        public string Type => DefaultComponent.Type;
        public string Value => DefaultComponent.Value;
        public bool HasColor => DefaultComponent.HasColor;

        public TextColor Color
        {
            get => DefaultComponent.Color;
            set => DefaultComponent.Color = value;
        }

        public TextStyles Styles
        {
            get => DefaultComponent.Styles;
            set => DefaultComponent.Styles = value;
        }

        public readonly ITextComponent DefaultComponent = new TextComponentString("");
        private readonly List<ITextComponent> _textComponents = new List<ITextComponent>();
        
        public void Reset(bool color = true, bool styles = true)
        {
            DefaultComponent.Reset(color, styles);
            foreach (var textComponent in _textComponents)
                textComponent.Reset(color, styles);
        }

        public ITextComponent Clear()
        {
            _textComponents.Clear();
            return this;
        }

        IEnumerator<ITextComponent> IEnumerable<ITextComponent>.GetEnumerator()
        {
            return _textComponents.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _textComponents.GetEnumerator();
        }

        public ITextComponent this[int index] => _textComponents[index];

        public static TextContainer operator +(TextContainer container, ITextComponent component)
        {
            container._textComponents.Add(component);
            return container;
        }
    }
}