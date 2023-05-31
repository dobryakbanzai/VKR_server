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
    public class ChallangesController : ControllerBase
    {
        private readonly PostgresContext _context;

        public ChallangesController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/Challanges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Challange>>> GetChallanges()
        {
            return await _context.Challanges.ToListAsync();
        }

        [Authorize]
        [HttpGet("mychallange")]
        public async Task<ActionResult<IEnumerable<Challange>>> GetSelfChallanges()
        {
            var req = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();

            Guid id = Guid.Parse(input: handler.ReadJwtToken(req)
                                               .Payload["id"]
                                               .ToString());

            var chal_stud = await _context.ChallangeStudents.Where(p => p.StudentId == id).ToListAsync();



            var chs = new List<Challange>();

            foreach (var item in chal_stud)
            {
                var a = await _context.Challanges.FindAsync(item.ChallangeId);
                var b = new Challange
                {
                    Id = a.Id,
                    ChallangeName = a.ChallangeName,
                    ChallangeType = a.ChallangeType,
                    ChallangeTarget = a.ChallangeTarget
                };
                chs.Add(b);
            }

            return chs;
        }

        [Authorize]
        [HttpPost("accch")]
        public async Task<ActionResult<IEnumerable<Challange>>> accePt(Guid challangeId)
        {
            var req = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();

            Guid id = Guid.Parse(input: handler.ReadJwtToken(req)
                                               .Payload["id"]
                                               .ToString());

            var challangeStudent = new ChallangeStudent();

            challangeStudent.Id = Guid.NewGuid();
            challangeStudent.StudentId = id;
            challangeStudent.ChallangeId = challangeId;

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

            return new List<Challange>();
        }


        [Authorize]
        [HttpGet("anotherchallange")]
        public async Task<ActionResult<IEnumerable<Challange>>> GetAnotherChallanges()
        {
            var req = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();

            Guid id = Guid.Parse(input: handler.ReadJwtToken(req)
                                               .Payload["id"]
                                               .ToString());

            var chal_stud = await _context.ChallangeStudents.Where(p => p.StudentId == id).ToListAsync();


            var chsall = await _context.Challanges.ToListAsync();

            var chs = new List<Challange>();

            foreach (var item in chal_stud)
            {
                var a = await _context.Challanges.FindAsync(item.ChallangeId);
                
                chs.Add(a);
            }

            var cheb = chsall.Except(chs).ToList();
            var che = new List<Challange>();

            foreach (var a in cheb)
            {
                var b = new Challange
                {
                    Id = a.Id,
                    ChallangeName = a.ChallangeName,
                    ChallangeType = a.ChallangeType,
                    ChallangeTarget = a.ChallangeTarget
                };
                che.Add(b);
            }


            return che;
        }

        // GET: api/Challanges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Challange>> GetChallange(Guid id)
        {
            var challange = await _context.Challanges.FindAsync(id);

            if (challange == null)
            {
                return NotFound();
            }

            return challange;
        }

        // PUT: api/Challanges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChallange(Guid id, Challange challange)
        {
            if (id != challange.Id)
            {
                return BadRequest();
            }

            _context.Entry(challange).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallangeExists(id))
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

        // POST: api/Challanges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Challange>> PostChallange(Challange challange)
        {
            
            challange.Id = Guid.NewGuid();

            _context.Challanges.Add(challange);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChallangeExists(challange.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChallange", new { id = challange.Id }, challange);
        }

        // DELETE: api/Challanges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChallange(Guid id)
        {
            var challange = await _context.Challanges.FindAsync(id);
            if (challange == null)
            {
                return NotFound();
            }

            _context.Challanges.Remove(challange);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChallangeExists(Guid id)
        {
            return _context.Challanges.Any(e => e.Id == id);
        }

        private bool ChallangeStudentExists(Guid id)
        {
            return (_context.ChallangeStudents?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }


}
