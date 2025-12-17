using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroSaldoFinal.Data;
using MicroSaldoFinal.Models;

namespace MicroSaldoFinal.Controllers
{
    public class IncomesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public IncomesController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Login", "Auth");

            var ingresos = await _db.Ingresos
                .Where(i => i.UsuarioId == userId.Value)
                .OrderByDescending(i => i.Fecha)
                .ToListAsync();
            return View(ingresos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string descripcion, decimal monto, DateTime fecha)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Login", "Auth");

            if (monto <= 0)
            {
                TempData["Error"] = "El monto debe ser mayor a 0.";
                return RedirectToAction("Index");
            }

            var ingreso = new Ingreso
            {
                UsuarioId = userId.Value,
                Monto = monto,
                Fecha = fecha,
                Descripcion = descripcion ?? string.Empty
            };

            _db.Ingresos.Add(ingreso);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Ingreso registrado.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Login", "Auth");

            var ingreso = await _db.Ingresos
                .FirstOrDefaultAsync(i => i.Id == id && i.UsuarioId == userId.Value);
            
            if (ingreso == null)
            {
                TempData["Error"] = "Ingreso no encontrado.";
                return RedirectToAction("Index");
            }

            _db.Ingresos.Remove(ingreso);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Ingreso eliminado.";
            return RedirectToAction("Index");
        }
    }
}
