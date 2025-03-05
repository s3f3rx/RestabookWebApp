using System.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestabookWebApp.Data;
using RestabookWebApp.Helpers;
using RestabookWebApp.Models;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
[Authorize]

public class ChefController(AppDbContext context, IWebHostEnvironment env) : Controller
{
    public IActionResult Index()
    {
        var chefs = context.Chefs.ToList();
        return View(chefs);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Chef? chef, IFormFile? file)
    {
        if (chef == null) return NotFound();
        if (!ModelState.IsValid) return View();
        if (file == null)
        {
            ModelState.AddModelError("error", "images are required!");
            return View();
        }

        chef.PhotoPath = await file.FileSaverAsync(env.WebRootPath, "chef");
        chef.CreatedDate = DateTime.Now;
        await context.Chefs.AddAsync(chef);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var chef = await context.Chefs.FindAsync(id);
        if (chef == null) return NotFound();
        return View(chef);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Chef? chef, IFormFile? file)
    {
        if (chef == null) return NotFound();
        if (!ModelState.IsValid) return View();
        if (file != null)
        {
            if (!string.IsNullOrEmpty(chef.PhotoPath))
            {
                var oldPhotoPath = Path.Combine(env.WebRootPath, chef.PhotoPath.TrimStart('/'));
                if (System.IO.File.Exists(oldPhotoPath))
                {
                    System.IO.File.Delete(oldPhotoPath);
                }
            }

            chef.PhotoPath = await file.FileSaverAsync(env.WebRootPath, "chef");
        }
        else
            ModelState.AddModelError("error", "images are not found!");

        chef.UpdatedDate = DateTime.Now;
        context.Chefs.Update(chef);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var chef = await context.Chefs.FindAsync(id);
        if (chef == null) return NotFound();
        return View(chef);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Chef? chef)
    {
        if (chef == null) return NotFound();

        var filePath = Path.Combine(env.WebRootPath, chef.PhotoPath.TrimStart('/'));
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);
        else
            ModelState.AddModelError("error", "Image is not valid or does not exist!");

        context.Chefs.Remove(chef);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}