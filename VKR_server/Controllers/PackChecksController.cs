using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VKR_server.Models;

namespace VKR_server.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class PackChecksController : ControllerBase
    {
        private readonly PostgresContext _context;

        public PackChecksController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/PackChecks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackCheck>>> GetPackChecks()
        {
          if (_context.PackChecks == null)
          {
              return NotFound();
          }
            return await _context.PackChecks.ToListAsync();
        }

        // GET: api/PackChecks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PackCheck>> GetPackCheck(Guid id)
        {
          if (_context.PackChecks == null)
          {
              return NotFound();
          }
            var packCheck = await _context.PackChecks.FindAsync(id);

            if (packCheck == null)
            {
                return NotFound();
            }

            return packCheck;
        }

        // PUT: api/PackChecks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPackCheck(Guid id, PackCheck packCheck)
        {
            if (id != packCheck.Id)
            {
                return BadRequest();
            }

            _context.Entry(packCheck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PackCheckExists(id))
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

        // POST: api/PackChecks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PackCheck>> PostPackCheck(PackCheck packCheck)
        {
          if (_context.PackChecks == null)
          {
              return Problem("Entity set 'PostgresContext.PackChecks'  is null.");
          }
            _context.PackChecks.Add(packCheck);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PackCheckExists(packCheck.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPackCheck", new { id = packCheck.Id }, packCheck);
        }

        // DELETE: api/PackChecks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackCheck(Guid id)
        {
            if (_context.PackChecks == null)
            {
                return NotFound();
            }
            var packCheck = await _context.PackChecks.FindAsync(id);
            if (packCheck == null)
            {
                return NotFound();
            }

            _context.PackChecks.Remove(packCheck);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PackCheckExists(Guid id)
        {
            return (_context.PackChecks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
