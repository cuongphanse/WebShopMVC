using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class CategoryController : Controller
{
    CategoryRepository categoryRepository;
    public CategoryController(IConfiguration configuration)
    {
        categoryRepository = new CategoryRepository(configuration);
    } 
    public IActionResult Index()
    {
        return View(categoryRepository.GetCategories());
    }

    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Add(Category category)
    {
        int result = categoryRepository.AddCategory(category);
        if (result > 0)
        {
            return Redirect("/category");
        }
        return View();
    }

    public IActionResult Edit(byte id)
    {
        return View(categoryRepository.GetCategoryById(id));
    }
    [HttpPost]
    public IActionResult Edit(Category category)
    {
        int result = categoryRepository.UpdateCategory(category);
        if (result > 0)
        {
            return Redirect("/category");
        }
        return Edit(category.Id);
    }
    public IActionResult Delete(byte id)
    {
        int result = categoryRepository.DeleteCategory(id);
        if (result > 0)
        {
            return Redirect("/category");
        }
        return Redirect("/category/error");
    }
}