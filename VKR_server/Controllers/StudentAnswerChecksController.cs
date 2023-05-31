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
    public class StudentAnswerChecksController : ControllerBase
    {
        private readonly PostgresContext _context;

        public StudentAnswerChecksController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/StudentAnswerChecks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentAnswerCheck>>> GetStudentAnswerChecks()
        {
          if (_context.StudentAnswerChecks == null)
          {
              return NotFound();
          }
            return await _context.StudentAnswerChecks.ToListAsync();
        }

        // GET: api/StudentAnswerChecks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentAnswerCheck>> GetStudentAnswerCheck(Guid id)
        {
          if (_context.StudentAnswerChecks == null)
          {
              return NotFound();
          }
            var studentAnswerCheck = await _context.StudentAnswerChecks.FindAsync(id);

            if (studentAnswerCheck == null)
            {
                return NotFound();
            }

            return studentAnswerCheck;
        }

        // PUT: api/StudentAnswerChecks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentAnswerCheck(Guid id, StudentAnswerCheck studentAnswerCheck)
        {
            if (id != studentAnswerCheck.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentAnswerCheck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentAnswerCheckExists(id))
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

        // POST: api/StudentAnswerChecks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentAnswerCheck>> PostStudentAnswerCheck(StudentAnswerCheck studentAnswerCheck)
        {
          if (_context.StudentAnswerChecks == null)
          {
              return Problem("Entity set 'PostgresContext.StudentAnswerChecks'  is null.");
          }
            _context.StudentAnswerChecks.Add(studentAnswerCheck);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentAnswerCheckExists(studentAnswerCheck.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudentAnswerCheck", new { id = studentAnswerCheck.Id }, studentAnswerCheck);
        }

        // DELETE: api/StudentAnswerChecks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentAnswerCheck(Guid id)
        {
            if (_context.StudentAnswerChecks == null)
            {
                return NotFound();
            }
            var studentAnswerCheck = await _context.StudentAnswerChecks.FindAsync(id);
            if (studentAnswerCheck == null)
            {
                return NotFound();
            }

            _context.StudentAnswerChecks.Remove(studentAnswerCheck);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentAnswerCheckExists(Guid id)
        {
            return (_context.StudentAnswerChecks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
