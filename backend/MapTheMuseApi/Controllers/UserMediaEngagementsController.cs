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
    public class UserMediaEngagementsController : ControllerBase
    {
        private readonly MapTheMuseContext _context;

        public UserMediaEngagementsController(MapTheMuseContext context)
        {
            _context = context;
        }

        // GET: api/UserMediaEngagements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMediaEngagement>>> GetUserMediaEngagement()
        {
            return await _context.UserMediaEngagements.ToListAsync();
        }

        // GET: api/UserMediaEngagements/5
        [HttpGet("{id}", Name = nameof(GetById))]
        public async Task<ActionResult<UserMediaEngagementReadDto>> GetById(int id)
        {
            var e = await _context.UserMediaEngagements
                .AsNoTracking()
                .Include(x => x.Destination)
                .Include(x => x.Media)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null)
                return NotFound();

            var dto = new UserMediaEngagementReadDto
            {
                Id = e.Id,
                DateVisited = e.DateVisited,
                Destination = new DestinationSummaryDto
                {
                    Id = e.Destination.Id,
                    Name = e.Destination.Name
                },
                Media = new MediaSummaryDto
                {
                    Id = e.Media.Id,
                    Title = e.Media.Title
                }
            };

            return Ok(dto);
        }

        // GET /api/usermediaengagements/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserMediaEngagementReadDto>>> GetByUser(string userId)
        {
            var list = await _context.UserMediaEngagements
                .AsNoTracking()
                .Where(e => e.UserId == userId)
                .Include(e => e.Destination)
                .Include(e => e.Media)
                .Select(e => new UserMediaEngagementReadDto
                {
                    Id = e.Id,
                    DateVisited = e.DateVisited,
                    Destination = new DestinationSummaryDto
                    {
                        Id = e.Destination.Id,
                        Name = e.Destination.Name
                    },
                    Media = new MediaSummaryDto
                    {
                        Id = e.Media.Id,
                        Title = e.Media.Title
                    }
                })
                .ToListAsync();

            return Ok(list);
        }

        // PUT: api/UserMediaEngagements/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserMediaEngagement(int id, UserMediaEngagement userMediaEngagement)
        {
            if (id != userMediaEngagement.Id)
            {
                return BadRequest();
            }

            _context.Entry(userMediaEngagement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserMediaEngagementExists(id))
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

        // POST /api/usermediaengagements
        [HttpPost]
        public async Task<ActionResult<UserMediaEngagementReadDto>> Post([FromBody] UserMediaEngagementCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1) Map to entity & stamp visit time
            var engagement = new UserMediaEngagement
            {
                UserId = dto.UserId,
                DestinationId = dto.DestinationId,
                MediaId = dto.MediaId,
                DateVisited = DateTime.UtcNow
            };

            // 2) Persist
            _context.UserMediaEngagements.Add(engagement);
            await _context.SaveChangesAsync();

            // 3) Reload with nav-props
            var loaded = await _context.UserMediaEngagements
                .AsNoTracking()
                .Include(e => e.Destination)
                .Include(e => e.Media)
                .FirstOrDefaultAsync(e => e.Id == engagement.Id)!;

            // 4) Project to ReadDto
            var readDto = new UserMediaEngagementReadDto
            {
                Id = loaded.Id,
                DateVisited = loaded.DateVisited,
                Destination = new DestinationSummaryDto
                {
                    Id = loaded.Destination.Id,
                    Name = loaded.Destination.Name
                },
                Media = new MediaSummaryDto
                {
                    Id = loaded.Media.Id,
                    Title = loaded.Media.Title
                }
            };

            // 5) Return 201 Created pointing to the “get by id” route
            return CreatedAtAction(
                nameof(GetById),
                new { id = readDto.Id },
                readDto
            );
        }
        private bool UserMediaEngagementExists(int id)
        {
            return _context.UserMediaEngagements.Any(e => e.Id == id);
        }
    }
}
