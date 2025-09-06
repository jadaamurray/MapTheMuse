using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MapTheMuseApi.Dtos;

namespace MapTheMuseApi.Controllers
{

    [ApiController]
    [Route("api/destinations/{destinationId:int}/media")]
    public class DestinationsMediaController : ControllerBase
    {
        private readonly IDestinationMediaService _svc;
        public DestinationsMediaController(IDestinationMediaService svc) => _svc = svc;

        private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<ActionResult<List<DestinationMediaItemDto>>> Get(int destinationId)
        {
            var items = await _svc.GetForDestinationAsync(destinationId, HttpContext.RequestAborted);
            return Ok(items);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Link(int destinationId, [FromBody] CreateDestinationMediaLinkDto dto)
        {
            try
            {
                var id = await _svc.LinkAsync(destinationId, dto, GetUserId(), HttpContext.RequestAborted);
                return CreatedAtAction(nameof(Get), new { destinationId }, new { id });
            }
            catch (InvalidOperationException ex) // duplicate, etc.
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("bulk")]
        public async Task<ActionResult> Bulk(int destinationId, [FromBody] List<CreateDestinationMediaLinkDto> items)
        {
            if (items is null || items.Count == 0)
                return BadRequest("Body must be a non-empty JSON array.");

            var createdIds = new List<int>();
            var duplicates = new List<object>();
            var errors = new List<object>();

            foreach (var dto in items)
            {
                try
                {
                    var id = await _svc.LinkAsync(destinationId, dto, GetUserId(), HttpContext.RequestAborted);
                    createdIds.Add(id);
                }
                catch (InvalidOperationException) // duplicate
                {
                    duplicates.Add(new { dto.Source, dto.ExternalId });
                }
                catch (Exception ex)
                {
                    errors.Add(new { dto.Source, dto.ExternalId, error = ex.Message });
                }
            }

            return Ok(new { created = createdIds.Count, createdIds, duplicates, errors });
        }

        // update context note
        [Authorize]
        [HttpPatch("{linkId:int}/note")]
        public async Task<ActionResult> UpdateNote(int destinationId, int linkId, [FromBody] string? note)
        {
            await _svc.UpdateContextNoteAsync(linkId, note, HttpContext.RequestAborted);
            return NoContent();
        }

        // reorder links (pass an array of { linkId, orderIndex })
        public record ReorderItem(int linkId, int? orderIndex);

        [Authorize]
        [HttpPost("reorder")]
        public async Task<ActionResult> Reorder(int destinationId, [FromBody] List<ReorderItem> order)
        {
            if (order is null) return BadRequest("Body required.");
            var pairs = order.Select(o => (o.linkId, o.orderIndex)).ToList();
            await _svc.ReorderAsync(destinationId, pairs, HttpContext.RequestAborted);
            return NoContent();
        }
    }

    [ApiController]
    [Route("api/destinations/media")]
    public class DestinationMediaLinksController : ControllerBase
    {
        private readonly IDestinationMediaService _svc;
        public DestinationMediaLinksController(IDestinationMediaService svc) => _svc = svc;

        [Authorize]
        [HttpDelete("{linkId:int}")]
        public async Task<ActionResult> Delete(int linkId)
        {
            await _svc.UnlinkAsync(linkId, HttpContext.RequestAborted);
            return NoContent();
        }
    }
}