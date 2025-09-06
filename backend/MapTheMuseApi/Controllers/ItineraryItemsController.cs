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

    public class ItineraryItemsController : ControllerBase
    {
        private readonly MapTheMuseContext _context;

        public ItineraryItemsController(MapTheMuseContext context)
        {
            _context = context;
        }

        // GET: api/ItineraryItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItineraryItemReadDto>>> GetItineraryItems()
        {
            var items = await _context.ItineraryItems
                            .AsNoTracking()
                            .Include(it => it.Destination)
                            .Include(it => it.PhysicalArt)
                            .Select(it => new ItineraryItemReadDto
                            {
                                Id = it.Id,
                                StartDate = it.StartDate,
                                EndDate = it.EndDate,
                                Order = it.Order,
                                Note = it.Note,

                                Destination = new DestinationSummaryDto
                                {
                                    Id = it.Destination.Id,
                                    Name = it.Destination.Name
                                },

                                PhysicalArt = it.PhysicalArt != null
                                    ? new PhysicalArtSummaryDto
                                    {
                                        Id = it.PhysicalArt.Id,
                                        Title = it.PhysicalArt.Title
                                    }
                                    : null
                            })
                            .ToListAsync();

            return Ok(items);
        }

        // GET: api/ItineraryItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItineraryItemReadDto>> GetById(int id)
        {
            var item = await _context.ItineraryItems
                 .AsNoTracking()
                 .Include(it => it.Destination)
                 .Include(it => it.PhysicalArt)
                 .Where(it => it.Id == id)
                 .Select(it => new ItineraryItemReadDto
                 {
                     Id = it.Id,
                     Destination = new DestinationSummaryDto
                     {
                         Id = it.Destination.Id,
                         Name = it.Destination.Name
                     },
                     StartDate = it.StartDate,
                     EndDate = it.EndDate,
                     Order = it.Order,
                     Note = it.Note,
                     PhysicalArt = it.PhysicalArt != null
                         ? new PhysicalArtSummaryDto
                         {
                             Id = it.PhysicalArt.Id,
                             Title = it.PhysicalArt.Title
                         }
                         : null
                 })
                 .FirstOrDefaultAsync();

            if (item == null)
                return NotFound();

            return Ok(item);
        }



        // PUT: api/ItineraryItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItineraryItem(int id, ItineraryItem itineraryItem)
        {
            if (id != itineraryItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(itineraryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItineraryItemExists(id))
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

        // POST: api/ItineraryItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ItineraryItem>> PostItineraryItem(ItineraryItemCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = new ItineraryItem
            {
                ItineraryId = dto.ItineraryId,
                DestinationId = dto.DestinationId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Order = dto.Order,
                Note = dto.Note
            };

            _context.ItineraryItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        // DELETE: api/ItineraryItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItineraryItem(int id)
        {
            var itineraryItem = await _context.ItineraryItems.FindAsync(id);
            if (itineraryItem == null)
            {
                return NotFound();
            }

            _context.ItineraryItems.Remove(itineraryItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItineraryItemExists(int id)
        {
            return _context.ItineraryItems.Any(e => e.Id == id);
        }
    }
}
