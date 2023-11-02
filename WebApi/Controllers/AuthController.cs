using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar(UsuarioDTO u)
        {
            Usuario U = new Usuario();
            U.Name = u.Name;
            U.Email = u.Email;
            U.Username = u.Username;

            CreatePasswordHash(u.Password, out byte[] hash, out byte[] salt);
            U.PasswordHash = hash;
            U.PasswordSalt = salt;

            _context.Add(U);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.TblUsuarios.Where(u => u.Username == username).FirstOrDefault();
            if (user == null) return BadRequest("Usuario NO Encontrado!");
            if(VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                string Token = GenerarToken(user);
                return Ok(Token);
            }
            return BadRequest("Password Incorrecta!");
        }



        //Metodo que recibe un String y Retorna Hash y Salt encriptado 1234
        private void CreatePasswordHash(string password, out byte[] passwordHash, 
            out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, 
            byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }



        private string GenerarToken(Usuario usuario)
        {
            List<Claim> datos = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario.Name),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, "Trabajador")
            };

            //12345678901234567890
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config.GetSection("AppSettings:TokenKey").Value));

            var Credential = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

            var Token = new JwtSecurityToken(
                claims: datos,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: Credential
            );

            var TokenGenerado = new JwtSecurityTokenHandler().WriteToken(Token);
            return TokenGenerado;
        }


    }
}
