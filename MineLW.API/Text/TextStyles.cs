using System;

namespace MineLW.API.Text
{
    [Flags]
    public enum TextStyles : byte
    {
        None = 0b00000000,
        Bold = 0b00000001,
        Italic = 0b00000010,
        Underlined = 0b00000100,
        Strikethrough = 0b00001000,
        Obfuscated = 0b00010000
    }
}