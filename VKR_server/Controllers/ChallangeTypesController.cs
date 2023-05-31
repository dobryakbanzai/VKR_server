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
    public class ChallangeTypesController : ControllerBase
    {
        private readonly PostgresContext _context;

        public ChallangeTypesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/ChallangeTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChallangeType>>> GetChallangeTypes()
        {
          if (_context.ChallangeTypes == null)
          {
              return NotFound();
          }
            return await _context.ChallangeTypes.ToListAsync();
        }

        // GET: api/ChallangeTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChallangeType>> GetChallangeType(Guid id)
        {
          if (_context.ChallangeTypes == null)
          {
              return NotFound();
          }
            var challangeType = await _context.ChallangeTypes.FindAsync(id);

            if (challangeType == null)
            {
                return NotFound();
            }

            return challangeType;
        }

        // PUT: api/ChallangeTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChallangeType(Guid id, ChallangeType challangeType)
        {
            if (id != challangeType.Id)
            {
                return BadRequest();
            }

            _context.Entry(challangeType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallangeTypeExists(id))
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

        // POST: api/ChallangeTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChallangeType>> PostChallangeType(ChallangeType challangeType)
        {
          if (_context.ChallangeTypes == null)
          {
              return Problem("Entity set 'PostgresContext.ChallangeTypes'  is null.");
          }
            _context.ChallangeTypes.Add(challangeType);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChallangeTypeExists(challangeType.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChallangeType", new { id = challangeType.Id }, challangeType);
        }

        // DELETE: api/ChallangeTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChallangeType(Guid id)
        {
            if (_context.ChallangeTypes == null)
            {
                return NotFound();
            }
            var challangeType = await _context.ChallangeTypes.FindAsync(id);
            if (challangeType == null)
            {
                return NotFound();
            }

            _context.ChallangeTypes.Remove(challangeType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChallangeTypeExists(Guid id)
        {
            return (_context.ChallangeTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
