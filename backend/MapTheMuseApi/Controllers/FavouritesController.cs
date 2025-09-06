using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MapTheMuseApi.Models;

namespace MapTheMuseApi.Controllers
{

    [ApiController]
    [Route("api/users/me/favourites")]
    public class FavouritesController : ControllerBase
    {
        private readonly IFavouritesService _svc;
        public FavouritesController(IFavouritesService svc) => _svc = svc;

        private string GetUserIdOrThrow() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException();

        // ---- Media ----
        [Authorize]
        [HttpGet("media")]
        public async Task<ActionResult> GetMyMedia([FromQuery] MediaType? type, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var list = await _svc.GetMyMediaAsync(GetUserIdOrThrow(), type, page, pageSize, HttpContext.RequestAborted);
            return Ok(list);
        }

        [Authorize]
        [HttpPost("media")]
        public async Task<ActionResult> FavouriteMedia([FromBody] FavouriteMediaRequestDto req)
        {
            await _svc.FavouriteMediaAsync(GetUserIdOrThrow(), req, HttpContext.RequestAborted);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("media/{mediaId:int}")]
        public async Task<ActionResult> UnfavouriteMediaById(int mediaId)
        {
            await _svc.UnfavouriteMediaByMediaIdAsync(GetUserIdOrThrow(), mediaId, HttpContext.RequestAborted);
            return NoContent();
        }

        // Optional: allow delete by external triple
        [Authorize]
        [HttpDelete("media/external/{source}/{type}/{externalId}")]
        public async Task<ActionResult> UnfavouriteMediaByExternal(string source, MediaType type, string externalId)
        {
            await _svc.UnfavouriteMediaByExternalAsync(GetUserIdOrThrow(), source, type, externalId, HttpContext.RequestAborted);
            return NoContent();
        }

        // ---- Destinations ----
        [Authorize]
        [HttpGet("destinations")]
        public async Task<ActionResult> GetMyDestinations([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var list = await _svc.GetMyDestinationsAsync(GetUserIdOrThrow(), page, pageSize, HttpContext.RequestAborted);
            return Ok(list);
        }

        [Authorize]
        [HttpPost("destinations/{destinationId:int}")]
        public async Task<ActionResult> FavouriteDestination(int destinationId)
        {
            await _svc.FavouriteDestinationAsync(GetUserIdOrThrow(), destinationId, HttpContext.RequestAborted);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("destinations/{destinationId:int}")]
        public async Task<ActionResult> UnfavouriteDestination(int destinationId)
        {
            await _svc.UnfavouriteDestinationAsync(GetUserIdOrThrow(), destinationId, HttpContext.RequestAborted);
            return NoContent();
        }
    }
}