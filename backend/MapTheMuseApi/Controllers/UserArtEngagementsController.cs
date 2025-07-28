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

namespace MapTheMuseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserArtEngagementsController : ControllerBase
    {
        private readonly MapTheMuseContext _context;

        public UserArtEngagementsController(MapTheMuseContext context)
        {
            _context = context;
        }

        // GET: api/UserArtEngagements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserArtEngagement>>> GetUserArtEngagement()
        {
            return await _context.UserArtEngagements.ToListAsync();
        }

        // GET: api/UserArtEngagements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserArtEngagement>> GetUserArtEngagement(int id)
        {
            var userArtEngagement = await _context.UserArtEngagements.FindAsync(id);

            if (userArtEngagement == null)
            {
                return NotFound();
            }

            return userArtEngagement;
        }
        
        // GET /api/userartengagements/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserArtEngagementReadDto>>> GetByUser(string userId)
        {
            var engagements = await _context.UserArtEngagements
                .AsNoTracking()
                .Where(e => e.UserId == userId)
                .Include(e => e.Destination)
                .Include(e => e.PhysicalArt)
                .Select(e => new UserArtEngagementReadDto
                {
                    Id = e.Id,
                    DateVisited = e.DateVisited,
                    Destination = new DestinationSummaryDto
                    {
                        Id = e.Destination.Id,
                        Name = e.Destination.Name
                    },
                    PhysicalArt = new PhysicalArtSummaryDto
                    {
                        Id = e.PhysicalArt.Id,
                        Title = e.PhysicalArt.Title
                    }
                })
                .ToListAsync();

            return Ok(engagements);
        }

        // PUT: api/UserArtEngagements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserArtEngagement(int id, UserArtEngagement userArtEngagement)
        {
            if (id != userArtEngagement.Id)
            {
                return BadRequest();
            }

            _context.Entry(userArtEngagement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserArtEngagementExists(id))
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

        // POST: api/UserArtEngagements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserArtEngagementCreateDto>> PostUserArtEngagement([FromBody] UserArtEngagementCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var engagement = new UserArtEngagement
            {
                UserId = dto.UserId,
                DestinationId = dto.DestinationId,
                PhysicalArtId = dto.PhysicalArtId,
                DateVisited = DateTime.UtcNow
            };

            _context.UserArtEngagements.Add(engagement);
            await _context.SaveChangesAsync();

            var loaded = await _context.UserArtEngagements
                .AsNoTracking()
                .Include(e => e.Destination)
                .Include(e => e.PhysicalArt)
                .FirstOrDefaultAsync(e => e.Id == engagement.Id)!;

            var readDto = new UserArtEngagementReadDto
            {
                Id = loaded.Id,
                DateVisited = loaded.DateVisited,

                Destination = new DestinationSummaryDto
                {
                    Id = loaded.Destination.Id,
                    Name = loaded.Destination.Name
                },

                PhysicalArt = new PhysicalArtSummaryDto
                {
                    Id = loaded.PhysicalArt.Id,
                    Title = loaded.PhysicalArt.Title
                }
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = readDto.Id },
                readDto
            );
        }

        /// <summary>
        /// GET /api/userartengagements/{id}
        /// Fetches a single engagement, used by CreatedAtAction above.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserArtEngagementReadDto>> GetById(int id)
        {
            var e = await _context.UserArtEngagements
                .AsNoTracking()
                .Include(x => x.Destination)
                .Include(x => x.PhysicalArt)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound();

            var dto = new UserArtEngagementReadDto
            {
                Id = e.Id,
                DateVisited = e.DateVisited,
                Destination = new DestinationSummaryDto
                {
                    Id = e.Destination.Id,
                    Name = e.Destination.Name
                },
                PhysicalArt = new PhysicalArtSummaryDto
                {
                    Id = e.PhysicalArt.Id,
                    Title = e.PhysicalArt.Title
                }
            };

            return Ok(dto);
        }

        // DELETE: api/UserArtEngagements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserArtEngagement(int id)
        {
            var userArtEngagement = await _context.UserArtEngagements.FindAsync(id);
            if (userArtEngagement == null)
            {
                return NotFound();
            }

            _context.UserArtEngagements.Remove(userArtEngagement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserArtEngagementExists(int id)
        {
            return _context.UserArtEngagements.Any(e => e.Id == id);
        }
    }
}
