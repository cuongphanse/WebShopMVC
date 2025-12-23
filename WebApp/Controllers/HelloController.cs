using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class HelloController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}