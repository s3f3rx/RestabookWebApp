using Microsoft.AspNetCore.Mvc;
using RestabookWebApp.Data;
using RestabookWebApp.Models;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
public class CategoryController(AppDbContext context) : Controller
{
    public IActionResult Index()
    {
        var categories = context.Categories.ToList();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category? category)
    {
        if (category == null) return NotFound();
         // if (!ModelState.IsValid) return View();

        category.CreatedDate = DateTime.Now;
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var category = await context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, Category? category)
    {
        if (id != category.Id || category == null) return NotFound();
        // if (!ModelState.IsValid) return View();

        var existingCategory = await context.Categories.FindAsync(id);
        if (existingCategory == null) return NotFound();

        if (existingCategory.Name == category.Name)
        {
            ModelState.AddModelError("", "No changes were made.");
            return View(category);
        }

        existingCategory.Name = category.Name;
        existingCategory.UpdatedDate = DateTime.Now;
        context.Update(existingCategory);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var category = await context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Category? category)
    {
        if (category == null) return NotFound();
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}