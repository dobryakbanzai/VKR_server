using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class TeachersController : ControllerBase
    {
        private readonly PostgresContext _context;

        public TeachersController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            return await _context.Teachers.ToListAsync();
        }

        [Authorize]
        [HttpGet("Myself")]
        public async Task<ActionResult<Teacher>> GetStudentSelf()
        {
            var req = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            //var id = new JwtSecurityToken(req).Payload["id"].ToString();
            var handler = new JwtSecurityTokenHandler();

            Guid id = Guid.Parse(handler.ReadJwtToken(req).Payload["id"].ToString());

            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            var teach_new = new Teacher
            {
                Id = teacher.Id,
                Firstname = teacher.Firstname,
                Lastname = teacher.Lastname,
                PersonRole = teacher.PersonRole,
                Experience = teacher.Experience
            };

            return teach_new;
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }

        // PUT: api/Teachers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(Guid id, Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest();
            }

            _context.Entry(teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
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

        // POST: api/Teachers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
        {
            teacher.Id = Guid.NewGuid();
            teacher.Pass = BCrypt.Net.BCrypt.HashPassword(teacher.Pass);
            foreach (var item in await _context.Students.ToListAsync())
            {
                if (item.Login == teacher.Login)
                {
                    return Conflict();
                }
            }
            foreach (var item in await _context.Teachers.ToListAsync())
            {
                if (item.Login == teacher.Login)
                {
                    return Conflict();
                }
            }

            _context.Teachers.Add(teacher);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TeacherExists(teacher.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTeacher", new { id = teacher.Id }, teacher);
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeacherExists(Guid id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
}
