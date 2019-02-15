using System;
using System.Collections.Generic;

// _Inicio_ Librerías para la generación de los jwt (JSON Web Tokens)
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
// _Fin_ Librerías para la generación de los jwt (JSON Web Tokens)
using System.Web;   
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using apiGTT.Models;
using apiGTT.Helpers;


namespace ApiGTT.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        // GET api/values
        public AuthController(AppDbContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] Users value)
        {

            try
            {

                Console.WriteLine("Recibido en el post ->>>>" + value.username);

                Users UserResult = this._context.Users.Where(
                   user => user.username == value.username).First();
                if (UserResult.password == Encrypt.Hash(value.password))
                {
                    Console.WriteLine("Login");
                    JwtSecurityToken token = BuildToken(UserResult);
                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception e)
            {
                return NotFound();
                // Console.WriteLine("Error ==> " + e.Message);

            }          
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /*public String BuildToken(Users data)
       {
           var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
           // Necesitamos conocer la fecha del sistema/actual
           var now = DateTime.UtcNow;
           var tokenDescriptor = new SecurityTokenDescriptor
           {
               Subject = new ClaimsIdentity(new[]
               {
                   new Claim( "UserID", data.id.ToString() ),
                   new Claim( "UserName", data.username ),
                   // para saber si es admin, podríamos hacer lo siguiente
                   new Claim( "Admin", "false" ),
                }),
               // Para indicarle la fecha de expiración del token
               Expires = now.AddDays(1),
               // Para indicar el tipo de encriptación a utilizar
               SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretsecretsecret")), SecurityAlgorithms.HmacSha256),
           };		
           var token = tokenHandler.CreateToken(tokenDescriptor);
           var tokenString = tokenHandler.WriteToken(token);
           return tokenString;
       } */


        public JwtSecurityToken BuildToken(Users data)
        {
            var claims = new[]{
              new Claim(ClaimTypes.Name, data.username),
              new Claim("id", data.id.ToString())
          };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("123456 secretsecretsecret"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "GTT .Inc",
                audience: "GTT .Inc",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            return token;
        }


    }
}