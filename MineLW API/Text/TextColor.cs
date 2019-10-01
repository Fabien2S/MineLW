﻿using System;
using System.Globalization;

namespace MineLW.API.Text
{
    public struct TextColor
    {
        public static readonly TextColor Black = new TextColor("black", '0');
        public static readonly TextColor DarkBlue = new TextColor("dark_blue", '1');
        public static readonly TextColor DarkGreen = new TextColor("dark_green", '2');
        public static readonly TextColor DarkAqua = new TextColor("dark_aqua", '3');
        public static readonly TextColor DarkRed = new TextColor("dark_red", '4');
        public static readonly TextColor DarkPurple = new TextColor("dark_purple", '5');
        public static readonly TextColor Gold = new TextColor("gold", '6');
        public static readonly TextColor Gray = new TextColor("gray", '7');
        public static readonly TextColor DarkGray = new TextColor("dark_gray", '8');
        public static readonly TextColor Blue = new TextColor("blue", '9');
        public static readonly TextColor Green = new TextColor("green", 'a');
        public static readonly TextColor Aqua = new TextColor("aqua", 'b');
        public static readonly TextColor Red = new TextColor("red", 'c');
        public static readonly TextColor LightPurple = new TextColor("light_purple", 'd');
        public static readonly TextColor Yellow = new TextColor("yellow", 'e');
        public static readonly TextColor White = new TextColor("white", 'f');

        public readonly string Name;
        public readonly char Code;
        public readonly int Id;

        private TextColor(string name, char code)
        {
            Name = name;
            Code = code;
            Id = Convert.ToInt32(code.ToString(CultureInfo.InvariantCulture), 16);
        }

        public override string ToString()
        {
            return Code.ToString();
        }
    }
}