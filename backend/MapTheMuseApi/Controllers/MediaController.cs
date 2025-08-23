using MapTheMuseApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class MediaController : ControllerBase
{
    private readonly IMediaService _svc;
    public MediaController(IMediaService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MediaListDto>>> GetMedia([FromQuery] int? skip, [FromQuery] int? take)
        => Ok(await _svc.GetAllAsync(skip, take));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MediaDetailDto>> GetById(int id)
    {
        var dto = await _svc.GetByIdAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] MediaCreateUpdateDto dto)
        => await _svc.UpdateAsync(id, dto) ? NoContent() : NotFound();

    [HttpPost]
    public async Task<ActionResult<MediaDetailDto>> Post([FromBody] MediaCreateUpdateDto dto)
    {
        var created = await _svc.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => await _svc.DeleteAsync(id) ? NoContent() : NotFound();
}
