using System.Text;

namespace MineLW.API.Extensions
{
    public static class StringExtensions
    {
        public static string ToUnderscoreCase(this string input)
        {
            if (input.Length == 0)
                return string.Empty;

            var builder = new StringBuilder();

            var c = input[0];
            builder.Append(char.IsUpper(c) ? char.ToLowerInvariant(c) : c);

            for (var i = 1; i < input.Length; i++)
            {
                c = input[i];
                if (char.IsUpper(c))
                {
                    builder
                        .Append('_')
                        .Append(char.ToLowerInvariant(c));
                }
                else
                    builder.Append(c);
            }

            return builder.ToString();
        }
    }
}