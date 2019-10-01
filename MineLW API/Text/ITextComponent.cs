﻿namespace MineLW.API.Text
{
    public interface ITextComponent
    {
        string Type { get; }
        string Value { get; }
        bool HasColor { get; }
        TextColor Color { get; set; }
        TextStyles Styles { get; set; }

        void Reset(bool color = true, bool styles = true);
    }
}