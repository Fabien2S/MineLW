using System.Linq;

namespace MineLW.API.Extensions
{
    public static class StringExtensions
    {
        public static string ToUnderscoreCase(this string input)
        {
            return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()));
        }
    }
}