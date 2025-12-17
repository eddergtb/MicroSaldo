using Microsoft.AspNetCore.Mvc;

namespace MicroSaldoFinal.Controllers
{
    public class HomePageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
