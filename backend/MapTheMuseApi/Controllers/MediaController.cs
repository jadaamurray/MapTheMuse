using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Data;
using MapTheMuseApi.Models;
using MapTheMuseApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace MapTheMuseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly MapTheMuseContext _context;

        public MediaController(MapTheMuseContext context)
        {
            _context = context;
        }

        // GET: api/Media
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaListDto>>> GetMedia()
        {
            var list = await _context.Media
                            .AsNoTracking()
                            .Select(m => new MediaListDto
                            {
                                Id = m.Id,
                                ExternalId = m.ExternalId,
                                Source = m.Source,
                                Title = m.Title,
                                ShortDescription =
                                    m.Description.Length <= 100
                                        ? m.Description
                                        : m.Description.Substring(0, 97) + "...",
                                Creator = m.Creator,
                                Type = m.Type,
                                ReleaseDate = m.ReleaseDate
                            })
                            .ToListAsync();

            return Ok(list);
        }

        // GET: api/Media/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MediaDetailDto>> GetbyId(int id)
        {
            var m = await _context.Media
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (m == null) return NotFound();

            var dto = new MediaDetailDto
            {
                Id = m.Id,
                ExternalId = m.ExternalId,
                Source = m.Source,
                Title = m.Title,
                Description = m.Description,
                Creator = m.Creator,
                Type = m.Type,
                ReleaseDate = m.ReleaseDate,
                PosterPath = m.PosterPath,
                LastSyncedUtc = m.LastSyncedUtc
            };

            return Ok(dto);
        }
        
        [Authorize(Roles = "Admin")]
        // PUT: api/Media/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedia(int id, Media media)
        {
            if (id != media.Id)
            {
                return BadRequest();
            }

            _context.Entry(media).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Media
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Media>> PostMedia(Media media)
        {
            _context.Media.Add(media);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedia", new { id = media.Id }, media);
        }

        // DELETE: api/Media/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedia(int id)
        {
            var media = await _context.Media.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }

            _context.Media.Remove(media);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MediaExists(int id)
        {
            return _context.Media.Any(e => e.Id == id);
        }
    }
}
