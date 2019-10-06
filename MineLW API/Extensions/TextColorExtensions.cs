using System;
using MineLW.API.Text;

namespace MineLW.API.Extensions
{
    public static class TextColorExtensions
    {
        private static readonly string[] ColorNames;

        static TextColorExtensions()
        {
            var names = Enum.GetNames(typeof(TextColor));
            var count = names.Length;

            ColorNames = new string[count];
            for (var i = 0; i < count; i++)
                ColorNames[i] = names[i].ToUnderscoreCase();
        }

        public static string GetName(this TextColor color)
        {
            return ColorNames[(byte) color];
        }
    }
}