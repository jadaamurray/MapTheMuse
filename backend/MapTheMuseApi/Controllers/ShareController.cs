using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MapTheMuseApi.Controllers
{
    [ApiController]
    [Route("share")]
    public class ShareController : Controller
    {
        private readonly CollageQuery _query;

        public ShareController(CollageQuery query)
        {
            _query = query;
        }

        // HTML share page for a user's collage (favourited destinations + media).
        // Social platforms read the OG meta; humans are redirected to the SPA profile.
        // Example: GET /share/user/7b7f7a2d-2b0b-4e5e-a2fd-9f6a9f5a1b23
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> User(string userId)
        {
            var (title, _, spaUrl) = await _query.ForUserAsync(userId);
            if (string.IsNullOrWhiteSpace(spaUrl))
                return NotFound(); // user not found or not shareable

            // Build the OG image URL; pass through a version param if present (forces re-scrape)
            var ogBase = $"https://api.mapthemuse.world/og/collage/user/{userId}.png";
            var ogImg = Request.Query.TryGetValue("v", out var v) && !string.IsNullOrWhiteSpace(v)
                ? $"{ogBase}?v={v}"
                : ogBase;

            var enc = HtmlEncoder.Default;

            var html = $@"<!doctype html>
<html lang=""en"">
<head>
<meta charset=""utf-8"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1"">

<!-- Open Graph -->
<meta property=""og:type"" content=""profile"">
<meta property=""og:title"" content=""{enc.Encode(title)}"">
<meta property=""og:description"" content=""My Travel Style · Map The Muse"">
<meta property=""og:url"" content=""{spaUrl}"">
<meta property=""og:image"" content=""{ogImg}"">
<meta property=""og:image:width"" content=""1200"">
<meta property=""og:image:height"" content=""630"">

<link rel=""canonical"" href=""{spaUrl}"">

<!-- Instant redirect for humans -->
<meta http-equiv=""refresh"" content=""0; url={spaUrl}"">
<title>{enc.Encode(title)}</title>
</head>
<body></body>
</html>";

            // Short cache helps social scrapers
            Response.Headers.CacheControl = "public, max-age=300, s-maxage=300";
            return Content(html, "text/html", Encoding.UTF8);
        }
    }
}
