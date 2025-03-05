using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestabookWebApp.Data;
using RestabookWebApp.Helpers;
using RestabookWebApp.Models;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
[Authorize]
public class ServiceController(AppDbContext context, IWebHostEnvironment env) : Controller
{
    public IActionResult Index()
    {
        var services = context.Services.ToList();
        return View(services);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Service? service, IFormFile? file)
    {
        if (file == null)
        {
            ModelState.AddModelError("error", "images are required!");
            return View();
        }

        if (!ModelState.IsValid) return View();
        if (service == null) return NotFound();

        service.PhotoPath = await file.FileSaverAsync(env.WebRootPath, "service");
        service.CreatedDate = DateTime.Now;
        await context.Services.AddAsync(service);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var service = await context.Services.FindAsync(id);
        if (service == null) return NotFound();
        return View(service);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Service? service, IFormFile? file)
    {
        if (service == null) return NotFound();
        if (!ModelState.IsValid) return View(service);
        if (file != null)
        {
            if (!string.IsNullOrEmpty(service.PhotoPath))
            {
                var oldPhotoPath = Path.Combine(env.WebRootPath, service.PhotoPath.TrimStart('/'));
                if (System.IO.File.Exists(oldPhotoPath))
                {
                    System.IO.File.Delete(oldPhotoPath);
                }
            }

            service.PhotoPath = await file.FileSaverAsync(env.WebRootPath, "service");
        }
        else
            ModelState.AddModelError("error", "images are not found!");

        service.UpdatedDate = DateTime.Now;
        context.Services.Update(service);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var service = await context.Services.FindAsync(id);
        if (service == null) return NotFound();
        return View(service);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Service? service)
    {
        if (service == null) return NotFound();

        var filePath = Path.Combine(env.WebRootPath, service.PhotoPath.TrimStart('/'));
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);
        else
                ModelState.AddModelError("error", "Image is not valid or does not exist!");

        context.Services.Remove(service);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}