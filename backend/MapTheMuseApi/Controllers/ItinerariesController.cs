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
    [Authorize]

    public class ItinerariesController : ControllerBase
    {
        private readonly MapTheMuseContext _context;

        public ItinerariesController(MapTheMuseContext context)
        {
            _context = context;
        }

        // GET: api/Itineraries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItineraryListDto>>> GetItineraries()
        {
            var list = await _context.Itineraries
                                                    .AsNoTracking()
                                                    .Select(i => new ItineraryListDto
                                                    {
                                                        Id = i.Id,
                                                        UserId = i.UserId,
                                                        Name = i.Name,
                                                        ShortDescription =
                                                            i.Description.Length <= 100
                                                                ? i.Description
                                                                : i.Description.Substring(0, 97) + "...",
                                                        CreatedAt = i.CreatedAt,
                                                        StartDate = i.StartDate,
                                                        EndDate = i.EndDate
                                                    })
                                                    .ToListAsync();

            return Ok(list);
        }

        // GET: api/Itineraries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItineraryDetailDto>> FindbyId(int id)
        {
            var itinerary = await _context.Itineraries
                .AsNoTracking()
                .Include(x => x.ItineraryItems)
            .ThenInclude(it => it.Destination)
        .Include(i => i.ItineraryItems)
            .ThenInclude(it => it.PhysicalArt)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (itinerary == null) return NotFound();

            var dto = new ItineraryDetailDto
            {
                Id = itinerary.Id,
                UserId = itinerary.UserId,
                Name = itinerary.Name,
                Description = itinerary.Description,
                CreatedAt = itinerary.CreatedAt,
                StartDate = itinerary.StartDate,
                EndDate = itinerary.EndDate,
                Items = itinerary.ItineraryItems
            .Select(it => new ItineraryItemReadDto
            {
                Id = it.Id,
                StartDate = it.StartDate,
                EndDate = it.EndDate,
                Order = it.Order,
                Note = it.Note,

                Destination = new DestinationSummaryDto
                {
                    Id = it.Destination!.Id,
                    Name = it.Destination.Name
                },

                PhysicalArt = new PhysicalArtSummaryDto
                {
                    Id = it.PhysicalArt!.Id,
                    Title = it.PhysicalArt.Title
                }
            })
            .ToList()
            };


            return Ok(dto);
        }

        // PUT: api/Itineraries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItinerary(int id, Itinerary itinerary)
        {
            if (id != itinerary.Id)
            {
                return BadRequest();
            }

            _context.Entry(itinerary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItineraryExists(id))
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

        // POST: api/Itineraries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItineraryCreateDto>> PostItinerary(ItineraryCreateDto dto)
        {
            var itinerary = new Itinerary
            {
                UserId = dto.UserId,
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
            _context.Itineraries.Add(itinerary);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItinerary", new { id = itinerary.Id }, itinerary);
        }

        // DELETE: api/Itineraries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItinerary(int id)
        {
            var itinerary = await _context.Itineraries.FindAsync(id);
            if (itinerary == null)
            {
                return NotFound();
            }

            _context.Itineraries.Remove(itinerary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItineraryExists(int id)
        {
            return _context.Itineraries.Any(e => e.Id == id);
        }
    }
}
