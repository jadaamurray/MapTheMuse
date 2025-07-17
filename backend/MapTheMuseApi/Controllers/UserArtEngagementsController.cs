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
            return await _context.UserArtEngagement.ToListAsync();
        }

        // GET: api/UserArtEngagements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserArtEngagement>> GetUserArtEngagement(int id)
        {
            var userArtEngagement = await _context.UserArtEngagement.FindAsync(id);

            if (userArtEngagement == null)
            {
                return NotFound();
            }

            return userArtEngagement;
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
        public async Task<ActionResult<UserArtEngagement>> PostUserArtEngagement(UserArtEngagement userArtEngagement)
        {
            _context.UserArtEngagement.Add(userArtEngagement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserArtEngagement", new { id = userArtEngagement.Id }, userArtEngagement);
        }

        // DELETE: api/UserArtEngagements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserArtEngagement(int id)
        {
            var userArtEngagement = await _context.UserArtEngagement.FindAsync(id);
            if (userArtEngagement == null)
            {
                return NotFound();
            }

            _context.UserArtEngagement.Remove(userArtEngagement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserArtEngagementExists(int id)
        {
            return _context.UserArtEngagement.Any(e => e.Id == id);
        }
    }
}
