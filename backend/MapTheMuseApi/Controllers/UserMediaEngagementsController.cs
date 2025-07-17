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
            return await _context.UserMediaEngagement.ToListAsync();
        }

        // GET: api/UserMediaEngagements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserMediaEngagement>> GetUserMediaEngagement(int id)
        {
            var userMediaEngagement = await _context.UserMediaEngagement.FindAsync(id);

            if (userMediaEngagement == null)
            {
                return NotFound();
            }

            return userMediaEngagement;
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

        // POST: api/UserMediaEngagements
        [HttpPost]
        public async Task<ActionResult<UserMediaEngagement>> PostUserMediaEngagement(UserMediaEngagement userMediaEngagement)
        {
            _context.UserMediaEngagement.Add(userMediaEngagement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserMediaEngagement", new { id = userMediaEngagement.Id }, userMediaEngagement);
        }

        // DELETE: api/UserMediaEngagements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserMediaEngagement(int id)
        {
            var userMediaEngagement = await _context.UserMediaEngagement.FindAsync(id);
            if (userMediaEngagement == null)
            {
                return NotFound();
            }

            _context.UserMediaEngagement.Remove(userMediaEngagement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserMediaEngagementExists(int id)
        {
            return _context.UserMediaEngagement.Any(e => e.Id == id);
        }
    }
}
