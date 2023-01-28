using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using VKR_server.Models;

/*var studs = await _context.Students.ToListAsync();
            var stud = await _context.Students.FirstOrDefaultAsync();
            foreach (var item in studs)
            {
                if(BCrypt.Net.BCrypt.Verify("hello bitch fsdve", item.Pass))
                {
                    stud = item;
                }
            }

            return stud;*/

namespace VKR_server.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTSettings _options;
        private readonly PostgresContext _context;

        //"login":"124","pass":"12634"
       
        public AccountController(IOptions<JWTSettings> optAccess, PostgresContext context)
        {
            _options = optAccess.Value;
            _context = context;
        }

        [HttpGet("GetToken")]
        public async Task<string> GetTokenAsync() 
        {
            var students = await _context.Students.ToListAsync();

            List<Claim> claims = new List<Claim>();

            foreach (var item in students)
            {
                if(item.Login == "124")
                {
                    var id = "id";
                    var pr = "personrole";

                    claims.Add(new Claim(id, item.Id.ToString()));
                    claims.Add(new Claim(pr, item.PersonRole));
                }
            }
            /*List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "Nataly"));
            claims.Add(new Claim("level", "123"));
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));*/

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)


                );

            /*var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var handler = new JwtSecurityTokenHandler();
            var res = handler.ReadJwtToken(token).ToString();*/

            return new JwtSecurityTokenHandler().WriteToken(jwt);
            //return IP;
        }

        [HttpPost("GetToken")]
        public async Task<ActionResult<string>> PostToken(LogPas logPas)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            var students = await _context.Students.ToListAsync();
            var student = await _context.Students.FirstOrDefaultAsync();

            var teachers = await _context.Teachers.ToListAsync();
            var teacher = await _context.Teachers.FirstOrDefaultAsync();

            List<Claim> claims = new List<Claim>();
            
            if (students != null)
            {
                foreach (var item in students)
                {
                    if (item.Login == logPas.login)
                    {
                        if (BCrypt.Net.BCrypt.Verify(logPas.password, item.Pass))
                        {
                            var id = "id";
                            var pr = "personrole";

                            claims.Add(new Claim(id, item.Id.ToString()));
                            claims.Add(new Claim(pr, item.PersonRole));
                        }
                        else
                        {
                            return BadRequest();
                        }



                    }
                }

            }
            else if(teachers != null)
            {
                foreach (var item in teachers)
                {
                    if (BCrypt.Net.BCrypt.Verify(logPas.password, item.Pass))
                    {
                        var id = "id";
                        var pr = "personrole";

                        claims.Add(new Claim(id, item.Id.ToString()));
                        claims.Add(new Claim(pr, item.PersonRole));

                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            else
            {
                return BadRequest();
            }

            
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
