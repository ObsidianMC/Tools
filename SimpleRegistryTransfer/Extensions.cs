using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleRegistryTransfer
{
    public static class Extensions
    {
        public static readonly Regex pattern = new(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");

        public static string ToSnakeCase(this string str) => string.Join("_", pattern.Matches(str)).ToLower();


        /// <summary>
        /// Trims resource tag from the start and removes '_' characters.
        /// </summary>
        public static string TrimResourceTag(this string value, bool keepUnderscores = false)
        {
            var values = value.Split(':');

            var resourceLocationLength = values[0].Length + 1;

            int length = value.Length - resourceLocationLength;

            if (!keepUnderscores)
                length -= value.Count(c => c == '_');

            return string.Create(length, value, (span, source) =>
            {
                int sourceIndex = resourceLocationLength;
                for (int i = 0; i < span.Length;)
                {
                    char sourceChar = source[sourceIndex];

                    if (keepUnderscores)
                    {
                        span[i] = sourceChar;
                        i++;
                    }
                    else if (sourceChar != '_')
                    {
                        span[i] = sourceChar;
                        i++;
                    }
                    sourceIndex++;
                }
            });
        }
    }
}
