using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestabookWebApp.Data;
using RestabookWebApp.Models;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
[Authorize]
public class TagController(AppDbContext context) : Controller
{
    public IActionResult Index()
    {
        var tags = context.Tags.ToList();
        return View(tags);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tag? tag)
    {
        if (tag == null) return NotFound();
        if (!ModelState.IsValid) return View(tag);

        tag.CreatedDate = DateTime.Now;
        await context.Tags.AddAsync(tag);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var tag = await context.Tags.FindAsync(id);
        if (tag == null) return NotFound();
        return View(tag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Tag? tag)
    {
        if (tag == null) return NotFound();
        if (!ModelState.IsValid) return View(tag);
        tag.UpdatedDate = DateTime.Now;
        context.Tags.Update(tag);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var tag = await context.Tags.FindAsync(id);
        if (tag == null) return NotFound();
        return View(tag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Tag? tag)
    {
        if (tag == null) return NotFound();
        context.Tags.Remove(tag);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}