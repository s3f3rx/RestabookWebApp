using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestabookWebApp.Data;
using RestabookWebApp.Helpers;
using RestabookWebApp.Models;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
[Authorize]
public class AboutController(AppDbContext context, IWebHostEnvironment env) : Controller
{
    public IActionResult Index()
    {
        var abouts = context.Abouts.ToList();
        return View(abouts);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(About? about, IFormFile? file)
    {
        if (file == null)
        {
            ModelState.AddModelError("error", "images are required!");
            return View();
        }

        if (!ModelState.IsValid) return View();
        if (about == null) return NotFound();

        about.PhotoPath = await file.FileSaverAsync(env.WebRootPath, "about");
        about.CreatedDate = DateTime.Now;
        await context.Abouts.AddAsync(about);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var about = await context.Abouts.FindAsync(id);
        if (about == null) return NotFound();
        return View(about);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(About? about, IFormFile? file)
    {
        if (about == null) return NotFound();
        if (!ModelState.IsValid) return View(about);
        
        if (file != null)
        {
            if (!string.IsNullOrEmpty(about.PhotoPath))
            {
                var oldPhotoPath = Path.Combine(env.WebRootPath, about.PhotoPath.TrimStart('/'));
                if (System.IO.File.Exists(oldPhotoPath))
                {
                    System.IO.File.Delete(oldPhotoPath);
                }
            }

            about.PhotoPath = await file.FileSaverAsync(env.WebRootPath, "about");
        }
        else
            ModelState.AddModelError("error", "images are not found!");

        about.UpdatedDate = DateTime.Now;
        context.Update(about);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var about = await context.Abouts.FindAsync(id);
        if (about == null) return NotFound();
        return View(about);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(About? about)
    {
        // if (!ModelState.IsValid) return View(about); // işləmir
        if (about == null) return NotFound();
        var filePath = Path.Combine(env.WebRootPath, about.PhotoPath.TrimStart('/'));

        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);
        else
        {
            ModelState.AddModelError("error", "Image is not valid or does not exist!");
            return View(about);
        }

        //delete from database
        context.Abouts.Remove(about);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}