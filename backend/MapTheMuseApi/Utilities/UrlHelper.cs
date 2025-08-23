public static class UrlHelper
{
    public static string? ToAbsolute(string? input, string baseUrl)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        // Already absolute?
        if (Uri.TryCreate(input, UriKind.Absolute, out var abs) &&
            (abs.Scheme == Uri.UriSchemeHttps || abs.Scheme == Uri.UriSchemeHttp))
            return abs.ToString();

        // Protocol-relative: //cdn.site/img.jpg
        if (input.StartsWith("//"))
            return "https:" + input;

        // Root-relative: /images/x.jpg
        if (input.StartsWith("/"))
            return baseUrl.TrimEnd('/') + input;

        // Plain relative: images/x.jpg
        return baseUrl.TrimEnd('/') + "/" + input;
    }
}
