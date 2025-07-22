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
    public class ItineraryItemsController : ControllerBase
    {
        private readonly MapTheMuseContext _context;

        public ItineraryItemsController(MapTheMuseContext context)
        {
            _context = context;
        }

        // GET: api/ItineraryItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItineraryItem>>> GetItineraryItems()
        {
            return await _context.ItineraryItems.ToListAsync();
        }

        // GET: api/ItineraryItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItineraryItem>> GetItineraryItem(int id)
        {
            var itineraryItem = await _context.ItineraryItems.FindAsync(id);

            if (itineraryItem == null)
            {
                return NotFound();
            }

            return itineraryItem;
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

            return CreatedAtAction(nameof(GetItineraryItem), new { id = item.Id }, item);
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
