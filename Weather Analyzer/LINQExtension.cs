using System.Collections.Generic;

namespace WeatherAnalyzer
{

    #nullable enable

    public static class LINQExtension
    {

        public static string StringMerge(this IEnumerable<string?> enumerable, string? separator)
        {
            return string.Join(separator, enumerable);
        }
    }

    #nullable disable

}