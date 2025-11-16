using Microsoft.AspNetCore.Mvc;

namespace EngWordApp.Controllers
{
    public class StartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
