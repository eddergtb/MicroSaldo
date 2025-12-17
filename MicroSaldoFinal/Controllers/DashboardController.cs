using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroSaldoFinal.Data;
using MicroSaldoFinal.Models;

namespace MicroSaldoFinal.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            
            if (!userId.HasValue)
                return RedirectToAction("Login", "Auth");

            var user = await _db.Usuarios.FindAsync(userId.Value);
            if (user is null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Auth");
            }

            var ingresos = await _db.Ingresos
                .Where(i => i.UsuarioId == userId.Value)
                .ToListAsync();

            var egresos = await _db.Egresos
                .Where(e => e.UsuarioId == userId.Value)
                .ToListAsync();

            var totalIngresos = ingresos.Sum(i => i.Monto);
            var totalEgresos = egresos.Sum(e => e.Monto);

            var vm = new Models.DashboardViewModel
            {
                Usuario = user,
                Ingresos = ingresos.OrderByDescending(i => i.Fecha).ToList(),
                Egresos = egresos.OrderByDescending(e => e.Fecha).ToList(),
                TotalIngresos = totalIngresos,
                TotalEgresos = totalEgresos
            };

            return View(vm);
        }
    }
}
