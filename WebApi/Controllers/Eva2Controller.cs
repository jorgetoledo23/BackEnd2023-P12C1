using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class Eva2Controller : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public Eva2Controller(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }


        [HttpPost]
        [Route("LoginIn")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginIn(string correo, string password)
        {
            //Validar si Existe un usuario
           if(_context.TblUsuarios.ToList().Count == 0)
           {
                var PrimerUsuario = new Usuario();
                PrimerUsuario.Rol = "SuperAdministrador";
                PrimerUsuario.Email = "Admin@Admin.cl";
                PrimerUsuario.isBlock = false;
                PrimerUsuario.Username = "SuperAdmin";
                PrimerUsuario.Name = "Administrador";
                CreatePasswordHash("123456", out byte[] hash, out byte[] salt);
                PrimerUsuario.PasswordHash = hash;
                PrimerUsuario.PasswordSalt = salt;
                _context.Add(PrimerUsuario);
                await _context.SaveChangesAsync();
           }

            var user = await _context.TblUsuarios.Where(u => u.Email.Equals(correo)).FirstOrDefaultAsync();
            if (user == null) return BadRequest("Usuario No Encontrado!");
            if (user.isBlock) return BadRequest("No Puedes Iniciar Sesion");
            if(VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                var Token = GenerarToken(user);
                return Ok(Token);
            }
            return BadRequest("Password Incorrecta");

        }


        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> getAllUsers()
        {
            return Ok(await _context.TblUsuarios.ToListAsync());
        }

        [HttpPost]
        [Route("AddUser")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> addUser(UsuarioDTO udto)
        {
            if (udto.Rol != "Administrador" && udto.Rol != "Asistente" && udto.Rol != "Vendedor") return BadRequest("Rol Incorrecto!");

            var user = new Usuario();
            user.Name = udto.Name;
            user.Email = udto.Email;
            user.Username = udto.Username;
            user.Rol = udto.Rol;
            CreatePasswordHash(udto.Password, out byte[] hash, out byte[] salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            user.isBlock = false;

            _context.Add(user);
            await _context.SaveChangesAsync();
            return Ok("Usuario Creado!");

        }

        [HttpPut]
        [Route("BlockUser")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> BlockUser(string correo)
        {
            var user = await _context.TblUsuarios
                .FirstOrDefaultAsync(u => u.Email == correo);
            if (user == null) return BadRequest("Usuario No Existe");
            if (user.Rol == "SuperAdministrador") return BadRequest("No puedes bloquear a un SuperAdministrador");
            user.isBlock = true;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok("Usuario Bloqueado");
        }

        [HttpPut]
        [Route("UnblockUser")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> UnblockUser(string correo)
        {
            var user = await _context.TblUsuarios
                .FirstOrDefaultAsync(u => u.Email == correo);
            if (user == null) return BadRequest("Usuario No Existe");
            user.isBlock = false;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok("Usuario Desbloqueado");
        }

        [HttpDelete]
        [Route("DeleteUser")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> DeleteUser(string correo)
        {
            var user = await _context.TblUsuarios
                .FirstOrDefaultAsync(u => u.Email == correo);
            if (user == null) return BadRequest("Usuario No Existe");
            if (user.Rol == "SuperAdministrador") return BadRequest("No puedes Eliminar a un SuperAdministrador");
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("Usuario Eliminado");
        }

        [HttpPut]
        [Route("ChangePassword")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> ChangePassword(string correo, string nuevapass)
        {
            var user = await _context.TblUsuarios
                .FirstOrDefaultAsync(u => u.Email == correo);
            if (user == null) return BadRequest("Usuario No Existe");
            
            //TODO: Si estoy logueado como SuperAdministrador, SI puedo cambiar contraseña del SuperAdministrador
            if (user.Rol == "SuperAdministrador") 
                return BadRequest("No puedes cambiar la contraseña de un SuperAdministrador");

            CreatePasswordHash(nuevapass, out byte[] hash, out byte[] salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok("Contraseña Actualizada");
        }


        [HttpPut]
        [Route("UpdateUser")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> UpdateUser(UsuarioDTO udto, string correo)
        {
            var user = await _context.TblUsuarios
                .FirstOrDefaultAsync(u => u.Email == correo);
            if (user == null) return BadRequest("Usuario No Existe");

            //TODO: Si estoy logueado como SuperAdministrador, SI puedo cambiar contraseña del SuperAdministrador
            if (user.Rol == "SuperAdministrador")
                return BadRequest("No puedes cambiar la contraseña de un SuperAdministrador");

            user.Name = udto.Name;
            user.Email = udto.Email;
            user.Username = udto.Username;
            user.Rol = udto.Rol;

            CreatePasswordHash(udto.Password, out byte[] hash, out byte[] salt);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok("Usuario Actualizadao");
        }




        private string GenerarToken(Usuario usuario)
        {
            //Informacion del Token
            List<Claim> datos = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario.Name),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol)
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

        private void CreatePasswordHash(string password, out byte[] passwordHash,
            out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash,
            byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
