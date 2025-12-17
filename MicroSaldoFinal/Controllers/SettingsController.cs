using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MicroSaldoFinal.Data;
using MicroSaldoFinal.Models;
using System.IO;
using System;
using System.Threading.Tasks;

namespace MicroSaldoFinal.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SettingsController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Login", "Auth");

            var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == userId.Value);
            if (user is null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IFormFile photo, string Nombre, string Email, string NewPassword, string ConfirmPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Login", "Auth");

            var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == userId.Value);
            if (user is null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }

            if (!string.IsNullOrWhiteSpace(Nombre)) user.Nombre = Nombre;
            if (!string.IsNullOrWhiteSpace(Email)) user.Email = Email;

            if (!string.IsNullOrWhiteSpace(NewPassword))
            {
                if (NewPassword == ConfirmPassword)
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NewPassword);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Las contraseñas no coinciden.");
                    return View(user);
                }
            }

            if (photo != null && photo.Length > 0)
            {
                // Validar tipo de archivo
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(photo.FileName).ToLower();
                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError(string.Empty, "Solo se permiten archivos de imagen (.jpg, .jpeg, .png, .gif).");
                    return View(user);
                }

                // Validar tamaño (máximo 5 MB)
                if (photo.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(string.Empty, "El archivo no debe exceder 5 MB.");
                    return View(user);
                }

                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "uploads");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid().ToString("N") + ext;
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }
                user.RutaFoto = "/images/uploads/" + fileName;
            }

            _db.Usuarios.Update(user);
            await _db.SaveChangesAsync();

            HttpContext.Session.SetString("UserName", user.Nombre);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserPhoto", user.RutaFoto ?? string.Empty);

            ViewData["SuccessMessage"] = "Perfil actualizado correctamente.";
            return View(user);
        }
    }
}
