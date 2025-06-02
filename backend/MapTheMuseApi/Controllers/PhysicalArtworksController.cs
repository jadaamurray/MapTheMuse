using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MapTheMuseApi.Data;
using MapTheMuseApi.Models;

namespace MapTheMuseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhysicalArtworksController : ControllerBase
    {
        private readonly MapTheMuseContext _context;

        public PhysicalArtworksController(MapTheMuseContext context)
        {
            _context = context;
        }

        // GET: api/PhysicalArtworks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhysicalArt>>> GetPhysicalArtworks()
        {
            return await _context.PhysicalArtworks.ToListAsync();
        }

        // GET: api/PhysicalArtworks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhysicalArt>> GetPhysicalArt(int id)
        {
            var physicalArt = await _context.PhysicalArtworks.FindAsync(id);

            if (physicalArt == null)
            {
                return NotFound();
            }

            return physicalArt;
        }

        // PUT: api/PhysicalArtworks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhysicalArt(int id, PhysicalArt physicalArt)
        {
            if (id != physicalArt.Id)
            {
                return BadRequest();
            }

            _context.Entry(physicalArt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhysicalArtExists(id))
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

        // POST: api/PhysicalArtworks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PhysicalArt>> PostPhysicalArt(PhysicalArt physicalArt)
        {
            _context.PhysicalArtworks.Add(physicalArt);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhysicalArt", new { id = physicalArt.Id }, physicalArt);
        }

        // DELETE: api/PhysicalArtworks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhysicalArt(int id)
        {
            var physicalArt = await _context.PhysicalArtworks.FindAsync(id);
            if (physicalArt == null)
            {
                return NotFound();
            }

            _context.PhysicalArtworks.Remove(physicalArt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhysicalArtExists(int id)
        {
            return _context.PhysicalArtworks.Any(e => e.Id == id);
        }
    }
}
