using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroSaldoFinal.Data;
using MicroSaldoFinal.Models;

namespace MicroSaldoFinal.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ExpensesController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Login", "Auth");

            var egresos = await _db.Egresos
                .Where(e => e.UsuarioId == userId.Value)
                .OrderByDescending(e => e.Fecha)
                .ToListAsync();
            return View(egresos);
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

            var egreso = new Egreso
            {
                UsuarioId = userId.Value,
                Monto = monto,
                Fecha = fecha,
                Descripcion = descripcion ?? string.Empty,
                ProductoId = null // Producto no se enlaza desde este formulario simple
            };

            _db.Egresos.Add(egreso);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Egreso registrado.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFromProductPurchase(string nombre, string descripcion, int cantidad, decimal costo)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (cantidad <= 0 || costo <= 0)
            {
                TempData["Error"] = "La cantidad y el costo deben ser mayores a cero.";
                return RedirectToAction("Index");
            }
            
            // 1. Crear el nuevo producto
            var producto = new Producto
            {
                UsuarioId = userId.Value,
                Nombre = nombre,
                Descripcion = descripcion,
                Precio = 0, // El precio de venta se establece después en la vista de Productos
                Stock = cantidad
            };
            _db.Productos.Add(producto);

            // 2. Crear el egreso asociado a la compra
            var egreso = new Egreso
            {
                UsuarioId = userId.Value,
                Monto = costo,
                Fecha = DateTime.Now,
                Descripcion = $"Compra de {cantidad} unidades de {nombre}",
                Producto = producto // Usar la propiedad de navegación en lugar del Id
            };
            _db.Egresos.Add(egreso);

            await _db.SaveChangesAsync();

            TempData["Success"] = "Compra de producto registrada con éxito. El producto ha sido agregado al inventario.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Login", "Auth");

            var egreso = await _db.Egresos
                .Include(e => e.Producto) // Incluir el producto asociado
                .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == userId.Value);
            
            if (egreso == null)
            {
                TempData["Error"] = "Egreso no encontrado.";
                return RedirectToAction("Index");
            }

            // Si el egreso está asociado a un producto (fue una compra), eliminar también el producto.
            if (egreso.Producto != null)
            {
                _db.Productos.Remove(egreso.Producto);
            }

            _db.Egresos.Remove(egreso);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Egreso eliminado.";
            return RedirectToAction("Index");
        }
    }
}
