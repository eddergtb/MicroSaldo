using Microsoft.AspNetCore.Mvc;

namespace MicroSaldoFinal.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (!HttpContext.Session.Keys.Contains("UserId"))
                return RedirectToAction("Login", "Auth");

            return View();
        }
    }
}
