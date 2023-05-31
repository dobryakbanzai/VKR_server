using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
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

        //"login":"qwerty","pass":"qwerty"

        public AccountController(IOptions<JWTSettings> optAccess, PostgresContext context)
        {
            _options = optAccess.Value;
            _context = context;
        }

        [HttpGet("GetToken")]
        public async Task<string> GetTokenAsync() 
        {
            var res = "";

            using (StreamReader reader = new StreamReader("C:\\Users\\lalal\\OneDrive\\Desktop\\Diplom\\ServerASP\\VKR_server\\VKR_server\\open_key.txt"))
            {
                string text = await reader.ReadToEndAsync();

                res += text;

            }

            return res;
            //return IP;
        }

        [HttpPost("GetToken")]
        public async Task<ActionResult<string>> PostToken(LogPas logPas)
        {
            
            var decrLog = AsymmetricEncryptionUtility.DecryptData(AsymmetricEncryptionUtility.stringToByteArr(logPas.login), "C:\\Users\\lalal\\OneDrive\\Desktop\\Diplom\\ServerASP\\VKR_server\\VKR_server\\asymmetric_key.txt");
            var decrPas = AsymmetricEncryptionUtility.DecryptData(AsymmetricEncryptionUtility.stringToByteArr(logPas.password), "C:\\Users\\lalal\\OneDrive\\Desktop\\Diplom\\ServerASP\\VKR_server\\VKR_server\\asymmetric_key.txt");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            var students = await _context.Students.ToListAsync();

            var teachers = await _context.Teachers.ToListAsync();

            List<Claim> claims = new List<Claim>();
            
            if (students != null)
            {
                foreach (var item in students)
                {
                    if (item.Login == decrLog)
                    {
                        if (BCrypt.Net.BCrypt.Verify(decrPas, item.Pass))
                        {
                            var id = "id";
                            var pr = "personrole";

                            claims.Add(new Claim(id, item.Id.ToString()));
                            claims.Add(new Claim(pr, item.PersonRole));

                        }
                    }
                }

            }
            if(teachers != null)
            {
                foreach (var item in teachers)
                {
                    if (item.Login == decrLog)
                    {
                        if (BCrypt.Net.BCrypt.Verify(decrPas, item.Pass))
                        {
                            var id = "id";
                            var pr = "personrole";

                            claims.Add(new Claim(id, item.Id.ToString()));
                            claims.Add(new Claim(pr, item.PersonRole));

                        }
                    }
                }
            }
            else
            {
                return decrLog + " " + decrPas;
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
        [HttpGet("GetRSACrypted")]
        public async Task<ActionResult<string>> rSACryptQwerty()
        {
            var res = "";
            var log = "qwerty";
            var pass = "qwerty";

            using (StreamReader reader = new StreamReader("C:\\Users\\lalal\\OneDrive\\Desktop\\Diplom\\ServerASP\\VKR_server\\VKR_server\\open_key.txt"))
            {
                string text = await reader.ReadToEndAsync();

                res += text;

            }

            var logEncr = AsymmetricEncryptionUtility.byteArrToString(AsymmetricEncryptionUtility.EncryptData(log, res));
            var passEncr = AsymmetricEncryptionUtility.byteArrToString(AsymmetricEncryptionUtility.EncryptData(pass, res));

            return "\"login\":\"" + logEncr + "\",\n\"password\":\"" + passEncr + "\"";
        }

        [Authorize]
        [HttpGet("myrole")]
        public async Task<ActionResult<string>> GetStudentRole()
        {
            var req = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

            //var id = new JwtSecurityToken(req).Payload["id"].ToString();
            var handler = new JwtSecurityTokenHandler();

            string role = handler.ReadJwtToken(req).Payload["personrole"].ToString();

            return role;
        }

    }

    

}
