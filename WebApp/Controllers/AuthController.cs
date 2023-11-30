using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> LoginIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginIn(LoginViewModel lvm)
        {
            var usuarios = _context.Usuarios.ToList();
            if (usuarios.Count == 0)
            {
                var superAdmin = new Usuario();
                superAdmin.isBlock = false;
                superAdmin.Name = "Admin";
                superAdmin.Email = "admin@admin.cl";
                superAdmin.Rol = "SuperAdministrador";
                CreatePasswordHash("123456", out byte[] hash, out byte[] salt);
                superAdmin.PasswordHash = hash;
                superAdmin.PasswordSalt = salt;
                _context.Add(superAdmin);
                await _context.SaveChangesAsync();
            }

            var user = _context.Usuarios.FirstOrDefault(u => u.Email.Equals(lvm.Email));
            if(user == null)
            {
                ModelState.AddModelError("", "Email no encontrado");
                return View();
            }
            if(VerifyPassword(lvm.Password, user.PasswordHash, user.PasswordSalt))
            {
                var claims = new List<Claim>(){
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Rol)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, 
                    new AuthenticationProperties { IsPersistent = true });

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Contraseña Incorrecta");
                return View();
            }

        }

        [HttpGet]
        public async Task<RedirectToActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("LoginIn", "Auth");
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