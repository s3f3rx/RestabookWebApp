using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestabookWebApp.Data;
using RestabookWebApp.Helpers;
using RestabookWebApp.Models;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
public class ProductController(AppDbContext context, IWebHostEnvironment env) : Controller
{
    public async Task<IActionResult> Index()
    {
        var products = await context.Products
            .Include(x => x.Category)
            .ToListAsync();
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        var categories = await context.Categories.ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product, IFormFile? file)
    {
        var categories = await context.Categories.ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        if (file == null && string.IsNullOrWhiteSpace(product.PhotoPath))
        {
            ModelState.AddModelError("PhotoPath", "Photo is required.");
            return View(product);
        }

        if (product.CategoryId == 0)
        {
            ModelState.AddModelError("CategoryId", "Please select a category.");
            return View(product);
        }

        if (product.DiscountPrice.HasValue && product.DiscountPrice >= product.Price)
        {
            ModelState.AddModelError("DiscountPrice",
                "Discount price cannot be greater than or the same as the regular price.");
            return View(product);
        }

        if (!ModelState.IsValid) return View(product);

        if (file != null)
            product.PhotoPath = await file.FileSaverAsync(env.WebRootPath, "product");

        product.CreatedDate = DateTime.Now;
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var product = await context.Products.FindAsync(id);
        if (product == null) return NotFound();
        var categories = await context.Categories.ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, Product? product, IFormFile? file)
    {
        if (id != product?.Id || product == null) return NotFound();
        var existingProduct = await context.Products.FindAsync(id);
        if (existingProduct == null) return NotFound();

        //dəyişmə yoxdursa error çıxarılsın
        if (existingProduct.Name == product.Name &&
            existingProduct.Description == product.Description &&
            existingProduct.Price == product.Price &&
            existingProduct.DiscountPrice == product.DiscountPrice &&
            existingProduct.CategoryId == product.CategoryId &&
            existingProduct.Expires == product.Expires &&
            existingProduct.PhotoPath == product.PhotoPath)
        {
            ModelState.AddModelError("", "No changes were made.");
            return View(product);
        }

        var categories = await context.Categories.ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        if (product.CategoryId == 0)
        {
            ModelState.AddModelError("CategoryId", "Please select a category.");
            return View(product);
        }

        if (product.DiscountPrice.HasValue && product.DiscountPrice >= product.Price)
        {
            ModelState.AddModelError("DiscountPrice",
                "Discount price cannot be greater than or the same as the regular price.");
            return View(product);
        }

        if (!ModelState.IsValid) return View(product);
        if (file != null)
            product.PhotoPath = await file.FileSaverAsync(env.WebRootPath, "product");

        //dəyişmə olarsa mənimsədilsin
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.DiscountPrice = product.DiscountPrice;
        existingProduct.CategoryId = product.CategoryId;
        existingProduct.Expires = product.Expires;
        existingProduct.PhotoPath = product.PhotoPath;
        existingProduct.UpdatedDate = DateTime.Now;
        context.Update(existingProduct);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var product = await context.Products.FindAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Product? product)
    {
        if (product == null) return NotFound();
        var filePath = Path.Combine(env.WebRootPath, product.PhotoPath.TrimStart('/'));
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);
        else
            ModelState.AddModelError("error", "Image is not valid or does not exist!");

        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}