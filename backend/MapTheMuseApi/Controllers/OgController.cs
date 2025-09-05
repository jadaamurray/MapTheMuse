using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MapTheMuseApi.Controllers
{
    [ApiController]
    [Route("og")]
    public class OgController : ControllerBase
    {
        private readonly CollageQuery _query;
        private readonly CollageRenderer _renderer;

        public OgController(CollageQuery query, CollageRenderer renderer)
        {
            _query = query;
            _renderer = renderer;
        }
/*      Open Graph image for a user's collage (favourited destinations + media).
        Example: GET /og/collage/user/7b7f7a2d-2b0b-4e5e-a2fd-9f6a9f5a1b23.png */
        [HttpGet("collage/user/{userId:guid}.png")]
        public async Task<IActionResult> CollageForUser(string userId, CancellationToken ct)
        {
            var (title, items, _) = await _query.ForUserAsync(userId);
            if (items is null || items.Count == 0)
                return NotFound("No images available for this user.");

            // Render the 1200x630 collage PNG
            var png = await _renderer.RenderAsync(items, title, ct: ct);
            if (png is null || png.Length == 0)
                return StatusCode(500, "Failed to render collage.");

            // Cache for a day; downstream CDNs can also cache (s-maxage)
            Response.Headers.CacheControl = "public, max-age=86400, s-maxage=86400";

            // strong validator if you pass ?v=… on the URL
            if (Request.Query.TryGetValue("v", out var v))
                Response.Headers.ETag = $"W/\"{v}\"";

            return File(png, "image/png");
        }
    }
}
