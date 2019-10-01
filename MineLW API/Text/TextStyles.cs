﻿using System;

namespace MineLW.API.Text
{
    [Flags]
    public enum TextStyles
    {
        None = 0,
        Reset = 1,
        Bold = 1 << 1,
        Italic = 1 << 2,
        Underlined = 1 << 3,
        Strikethrough = 1 << 4,
        Obfuscated = 1 << 5,
    }
}