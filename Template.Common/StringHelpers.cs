using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Template.Common.Types;

namespace Template.Common
{
    public static class StringHelpers
    {
        public static string CoverWith(this string s, string pre, string post = default)
        {
            var sb = new StringBuilder(s);
            sb.Insert(0, pre);
            sb.Append(post ?? pre);

            return sb.ToString();
        }

        public static Languages AcceptLanguageToLanguage(string acceptLanguage)
        {
            if (string.IsNullOrWhiteSpace(acceptLanguage))
            {
                return Languages.English;
            }

            var languages = acceptLanguage.ToLower().Split(',')
                .Select(StringWithQualityHeaderValue.Parse)
                .OrderByDescending(s => s.Quality.GetValueOrDefault(1));

            foreach (var language in languages)
            {
                if (language.Value.StartsWith("en") || language.Value is "en-us" or "en")
                {
                    return Languages.English;
                }

                if (language.Value.StartsWith("tr") || language.Value is "tr-tr" or "tr")
                {
                    return Languages.Turkish;
                }
            }

            return Languages.English;
        }

    }
}