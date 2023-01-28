using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VKR_server.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using NuGet.Common;

namespace VKR_server.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        
        private readonly PostgresContext _context;

        public StudentsController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        [Authorize]
        [HttpGet("Myself")]
        public async Task<ActionResult<Student>> GetStudentSelf()
        {
            var req = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            
            //var id = new JwtSecurityToken(req).Payload["id"].ToString();
            var handler = new JwtSecurityTokenHandler();

            Guid id = Guid.Parse(handler.ReadJwtToken(req).Payload["id"].ToString());

            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            var stud_new = new Student
            {
                Id = student.Id,
                Firstname = student.Firstname,
                Lastname = student.Lastname,
                PersonRole = student.PersonRole,
                EdClass = student.EdClass,
                TeacherId = student.TeacherId,
                AryProg = student.AryProg,
                DerProg = student.DerProg,
                TasksProg = student.TasksProg
            };

            return stud_new;
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(Guid id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            student.Id = Guid.NewGuid();
            student.Pass = BCrypt.Net.BCrypt.HashPassword(student.Pass);
            foreach (var item in await _context.Students.ToListAsync())
            {
                if(item.Login == student.Login)
                {
                    return Conflict();
                }
            }
            foreach (var item in await _context.Teachers.ToListAsync())
            {
                if (item.Login == student.Login)
                {
                    return Conflict();
                }
            }
            _context.Students.Add(student);
            
            try
            {
                
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentExists(student.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
