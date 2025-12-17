using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroSaldoFinal.Data;
using MicroSaldoFinal.Models;

namespace MicroSaldoFinal.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Login", "Auth");

            var products = await _db.Productos
                .Where(p => p.UsuarioId == userId.Value)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string nombre, string descripcion, int cantidad, decimal precio)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Login", "Auth");

            if (string.IsNullOrWhiteSpace(nombre))
            {
                TempData["Error"] = "El nombre es obligatorio.";
                return RedirectToAction("Index");
            }

            var producto = new Producto
            {
                UsuarioId = userId.Value,
                Nombre = nombre,
                Descripcion = descripcion,
                Precio = precio,
                Stock = cantidad
            };

            _db.Productos.Add(producto);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Producto agregado.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sell(int id, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId is null)
                return RedirectToAction("Login", "Auth");

            var producto = await _db.Productos.FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == userId.Value);
            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado.";
                return RedirectToAction("Index");
            }

            if (producto.Precio <= 0)
            {
                TempData["Error"] = $"El producto '{producto.Nombre}' no tiene un precio de venta asignado.";
                return RedirectToAction("Index");
            }

            if (producto.Stock < quantity)
            {
                TempData["Error"] = "No hay stock disponible para vender.";
                return RedirectToAction("Index");
            }

            // 1. Decrement stock
            producto.Stock -= quantity;

            // 2. Create a new income record
            var ingreso = new Ingreso
            {
                UsuarioId = userId.Value,
                Monto = producto.Precio * quantity,
                Descripcion = $"Venta de {quantity} {producto.Nombre}",
                Fecha = DateTime.Now
            };

            _db.Ingresos.Add(ingreso);
            
            // 3. Save both changes
            await _db.SaveChangesAsync();

            TempData["Success"] = $"{quantity} '{producto.Nombre}' vendidos. Se agregó un ingreso de {(producto.Precio * quantity):C}.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, string nombre, string descripcion, int stock, decimal precio)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (string.IsNullOrWhiteSpace(nombre) || stock < 0 || precio < 0)
            {
                TempData["Error"] = "Los datos del producto no son válidos.";
                return RedirectToAction("Index");
            }

            var producto = await _db.Productos.FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == userId.Value);
            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado.";
                return RedirectToAction("Index");
            }

            producto.Nombre = nombre;
            producto.Descripcion = descripcion;
            producto.Stock = stock;
            producto.Precio = precio;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Producto actualizado con éxito.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var producto = await _db.Productos
                .Include(p => p.Egresos) // Cargar los egresos asociados
                .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == userId.Value);

            if (producto == null)
            {
                TempData["Error"] = "Producto no encontrado.";
                return RedirectToAction("Index");
            }

            // Antes de eliminar el producto, desvincula cualquier egreso asociado.
            // Esto evita el error de clave foránea.
            foreach (var egreso in producto.Egresos)
            {
                egreso.ProductoId = null;
            }

            // Ahora sí podemos eliminar el producto de forma segura.
            _db.Productos.Remove(producto);
            await _db.SaveChangesAsync();

            TempData["Success"] = $"Producto '{producto.Nombre}' eliminado con éxito.";
            return RedirectToAction("Index");
        }

        [HttpGet("Products/Delete")]
        public IActionResult Delete()
        {
            // Redirige si alguien intenta acceder a la URL de borrado directamente.
            TempData["Error"] = "La eliminación de productos debe realizarse a través del botón correspondiente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
