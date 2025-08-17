using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MapTheMuseApi.Dtos;

[ApiController]
[Route("api/destinations/{destinationId:int}/media")]
public class DestinationsMediaController : ControllerBase
{
    private readonly IDestinationMediaService _svc;

    public DestinationsMediaController(IDestinationMediaService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<List<DestinationMediaItemDto>>> Get(int destinationId)
        => Ok(await _svc.GetForDestinationAsync(destinationId));

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Link(int destinationId, [FromBody] CreateDestinationMediaLinkDto dto)
    {
        var id = await _svc.LinkAsync(destinationId, dto, User.Identity?.Name);
        return CreatedAtAction(nameof(Get), new { destinationId }, new { id });
    }
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
                var id = await _svc.LinkAsync(destinationId, dto, User.Identity?.Name);
                createdIds.Add(id);
            }
            catch (InvalidOperationException) // e.g. your service throws on duplicates
            {
                duplicates.Add(new { dto.Source, dto.ExternalId });
            }
            catch (Exception ex)
            {
                errors.Add(new { dto.Source, dto.ExternalId, error = ex.Message });
            }
        }

        return Ok(new
        {
            created = createdIds.Count,
            createdIds,
            duplicates,
            errors
        });
    }
}


// separate route for unlink
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
        await _svc.UnlinkAsync(linkId);
        return NoContent();
    }
}
