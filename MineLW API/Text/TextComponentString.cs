﻿namespace MineLW.API.Text
{
    public struct TextComponentString : ITextComponent
    {
        public string Type => "text";
        public string Value { get; }

        public bool HasColor => _color.HasValue;

        public TextColor Color
        {
            get => _color ?? TextColor.White;
            set => _color = value;
        }

        public TextStyles Styles { get; set; }

        private TextColor? _color;

        public TextComponentString(string text = "")
        {
            Value = text;
            Styles = TextStyles.None;

            _color = null;
        }

        public void Reset(bool color = true, bool styles = true)
        {
            if (styles) Styles = TextStyles.None;
            if (color) _color = null;
        }
    }
}