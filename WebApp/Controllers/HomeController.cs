using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{
    CategoryRepository categoryRepository;
    public HomeController(IConfiguration configuration)
    {
        categoryRepository = new CategoryRepository(configuration);
    }

    public IActionResult Index()
    {
        ViewBag.Categories = categoryRepository.GetCategories();
        return View();
    }
}