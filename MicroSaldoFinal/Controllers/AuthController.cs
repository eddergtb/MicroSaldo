using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroSaldoFinal.Data;
using MicroSaldoFinal.Models;

namespace MicroSaldoFinal.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AuthController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Email y contraseña son obligatorios.");
                return View();
            }

            var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Nombre);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserPhoto", user.RutaFoto ?? string.Empty);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string nombre, string apellidos, string documento, string telefono, string email, string password, string confirmarPassword)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Nombre, email y contraseña son obligatorios.");
                return View();
            }

            if (password != confirmarPassword)
            {
                ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                return View();
            }

            if (await _db.Usuarios.AnyAsync(u => u.Email == email))
            {
                ModelState.AddModelError(string.Empty, "El email ya está registrado.");
                return View();
            }

            var user = new Usuario
            {
                Nombre = nombre,
                Apellidos = apellidos ?? string.Empty,
                Documento = documento ?? string.Empty,
                Telefono = telefono ?? string.Empty,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RutaFoto = "/images/user.png"
            };

            _db.Usuarios.Add(user);
            await _db.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Logout")]
        public IActionResult LogoutPost()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
