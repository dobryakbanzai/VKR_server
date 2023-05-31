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
    public class ChallangeStudentsController : ControllerBase
    {
        private readonly PostgresContext _context;

        public ChallangeStudentsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/ChallangeStudents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChallangeStudent>>> GetChallangeStudents()
        {
          if (_context.ChallangeStudents == null)
          {
              return NotFound();
          }
            return await _context.ChallangeStudents.ToListAsync();
        }

        // GET: api/ChallangeStudents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChallangeStudent>> GetChallangeStudent(Guid id)
        {
          if (_context.ChallangeStudents == null)
          {
              return NotFound();
          }
            var challangeStudent = await _context.ChallangeStudents.FindAsync(id);

            if (challangeStudent == null)
            {
                return NotFound();
            }

            return challangeStudent;
        }

        // PUT: api/ChallangeStudents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChallangeStudent(Guid id, ChallangeStudent challangeStudent)
        {
            if (id != challangeStudent.Id)
            {
                return BadRequest();
            }

            _context.Entry(challangeStudent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallangeStudentExists(id))
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

        // POST: api/ChallangeStudents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChallangeStudent>> PostChallangeStudent(ChallangeStudent challangeStudent)
        {
            challangeStudent.Id = Guid.NewGuid();

            if (_context.ChallangeStudents == null)
          {
              return Problem("Entity set 'PostgresContext.ChallangeStudents'  is null.");
          }
            _context.ChallangeStudents.Add(challangeStudent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChallangeStudentExists(challangeStudent.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChallangeStudent", new { id = challangeStudent.Id }, challangeStudent);
        }
        /*
        [HttpPost("n")]
        public async Task<ActionResult<ChallangeStudent>> PostChallangeStudentn(ChallangeStudentBuff challangeStudent)
        {
            challangeStudent.Id = Guid.NewGuid();

            var chstd = new ChallangeStudent(challangeStudent.Id, challangeStudent.ChallangeId, challangeStudent.StudentId);

            if (_context.ChallangeStudents == null)
            {
                return Problem("Entity set 'PostgresContext.ChallangeStudents'  is null.");
            }
            _context.ChallangeStudents.Add(chstd);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChallangeStudentExists(challangeStudent.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChallangeStudent", new { id = challangeStudent.Id }, challangeStudent);
        }
        */
        // DELETE: api/ChallangeStudents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChallangeStudent(Guid id)
        {
            if (_context.ChallangeStudents == null)
            {
                return NotFound();
            }
            var challangeStudent = await _context.ChallangeStudents.FindAsync(id);
            if (challangeStudent == null)
            {
                return NotFound();
            }

            _context.ChallangeStudents.Remove(challangeStudent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChallangeStudentExists(Guid id)
        {
            return (_context.ChallangeStudents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
