using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroSaldoFinal.Data;
using MicroSaldoFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroSaldoFinal.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReportsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Default to the last 30 days if no dates are provided
            var finalEndDate = endDate ?? DateTime.Today;
            var finalStartDate = startDate ?? finalEndDate.AddDays(-29);

            var ingresosQuery = _db.Ingresos
                .Where(i => i.UsuarioId == userId.Value && i.Fecha.Date >= finalStartDate.Date && i.Fecha.Date <= finalEndDate.Date);

            var egresosQuery = _db.Egresos
                .Where(e => e.UsuarioId == userId.Value && e.Fecha.Date >= finalStartDate.Date && e.Fecha.Date <= finalEndDate.Date);

            var totalIngresosPeriodo = await ingresosQuery.SumAsync(i => i.Monto);
            var totalEgresosPeriodo = await egresosQuery.SumAsync(e => e.Monto);

            var ingresosPorDia = await ingresosQuery
                .GroupBy(i => i.Fecha.Date)
                .ToDictionaryAsync(g => g.Key, g => g.Sum(i => i.Monto));

            var egresosPorDia = await egresosQuery
                .GroupBy(e => e.Fecha.Date)
                .ToDictionaryAsync(g => g.Key, g => g.Sum(e => e.Monto));

            var allDates = ingresosPorDia.Keys.Union(egresosPorDia.Keys).OrderByDescending(d => d);

            var dailyReports = allDates.Select(date => new ReportViewModel
            {
                Fecha = date,
                TotalIngresos = ingresosPorDia.GetValueOrDefault(date, 0),
                TotalEgresos = egresosPorDia.GetValueOrDefault(date, 0)
            }).ToList();

            var model = new FinancialReportViewModel
            {
                DailyReports = dailyReports,
                TotalIngresosPeriodo = totalIngresosPeriodo,
                TotalEgresosPeriodo = totalEgresosPeriodo,
                StartDate = finalStartDate,
                EndDate = finalEndDate
            };

            return View(model);
        }
    }
}
