﻿using System;

 namespace MineLW.API.Text
{
    [Flags]
    public enum TextStyles
    {
        None = 0b00000000,
        Reset = 0b00000001,
        Bold = 0b00000010,
        Italic = 0b00000100,
        Underlined = 0b00001000,
        Strikethrough = 0b00010000,
        Obfuscated = 0b00100000
    }
}