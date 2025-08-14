using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MapTheMuseApi.Infrastructure.Text
{
    public static class Slugify
    {
        private static readonly Regex NonWord = new("[^a-z0-9\\- ]+", RegexOptions.Compiled);
        private static readonly Regex Spaces = new("\\s+", RegexOptions.Compiled);
        private static readonly Regex Dashes = new("\\-+", RegexOptions.Compiled);

        public static string From(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            // lower-case and remove accents
            var normalized = input.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark) sb.Append(c);
            }
            var s = sb.ToString().Normalize(NormalizationForm.FormC);
            s = Spaces.Replace(s, "-");            // spaces -> hyphens
            s = NonWord.Replace(s, "");            // remove punctuation/symbols
            s = Dashes.Replace(s, "-").Trim('-');  // collapse/trim hyphens
            return s;
        }
    }
}