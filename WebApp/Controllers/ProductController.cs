using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

public class ProductController : Controller
{
    ProductRepository productRepository;
    CategoryRepository categoryRepository;
    public ProductController(IConfiguration configuration)
    {
        productRepository = new ProductRepository(configuration);
        categoryRepository = new CategoryRepository(configuration);
    }

    public IActionResult Index()
    {
        return View(productRepository.GetProducts());
    }
    public IActionResult Add()
    {
        ViewBag.categories = categoryRepository.GetCategories();
        return View();
    }
    [HttpPost]
    public IActionResult Add(Product obj, IFormFile f)
    {
        ModelState.Remove(nameof(obj.ImageUrl));
        if(ModelState.IsValid && f != null)
        {
            string ext = Path.GetExtension(f.FileName);
            string imageUrl = Helper.RamdomString(32 -ext.Length) + ext;
            string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","images",imageUrl);
            using(Stream stream = new FileStream(path, FileMode.Create))
            {
                f.CopyTo(stream);
            }
            obj.ImageUrl = imageUrl;
            int ret = productRepository.Add(obj);
            if(ret > 0) return Redirect("/product");
            ModelState.AddModelError("Error", "Cannot add new product.");
        }
        ViewBag.categories = categoryRepository.GetCategories();
        return View(obj);
    }
  
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        Product? product = productRepository.GetProduct(id ?? 0);
        ViewBag.categories = categoryRepository.GetCategories();
        return View(product);
    }
    [HttpPost]
    public IActionResult Edit(Product obj, IFormFile? f)
    {
        ModelState.Remove(nameof(obj.ImageUrl));
        if (ModelState.IsValid)
        {
            if (f != null && f.Length > 0)
            {
                string ext = Path.GetExtension(f.FileName);
                string imageUrl = Helper.RamdomString(32 - ext.Length) + ext;
                string path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot", "images", imageUrl
                );
                if (!string.IsNullOrEmpty(obj.ImageUrl))
                {
                    string oldPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot", "images", obj.ImageUrl
                    );

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    f.CopyTo(stream);
                }
                obj.ImageUrl = imageUrl;
            }
            int ret = productRepository.Update(obj);
            if (ret > 0)
            {
                return Redirect("/product");
            }

            ModelState.AddModelError("", "Cannot update product.");
        }

        ViewBag.categories = categoryRepository.GetCategories();
        return View(obj);
    }
    public IActionResult Delete(int id)
    {
        return View(productRepository.GetProduct(id));
    }
    [HttpPost]
    public IActionResult Delete(int id, Product obj)
    {
        if(string.IsNullOrEmpty(obj.ImageUrl)) return Redirect("/product");
        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", obj.ImageUrl);
        if(System.IO.File.Exists(path)) System.IO.File.Delete(path);
        if(productRepository.Delete(id) > 0) return Redirect("/product");
        return Redirect("/product/error");
    }

}