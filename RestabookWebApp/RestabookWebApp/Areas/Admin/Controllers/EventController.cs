using Microsoft.AspNetCore.Mvc;
using RestabookWebApp.Data;
using RestabookWebApp.Models;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
public class EventController(AppDbContext context) : Controller
{
    public IActionResult Index()
    {
        var events = context.Events.ToList();
        return View(events);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Event? @event)
    {
        if (@event == null) return NotFound();
        if (!ModelState.IsValid) return View();

        @event.CreatedDate = DateTime.Now;
        await context.AddAsync(@event);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var @event = await context.Events.FindAsync(id);
        if (@event == null) return NotFound();
        return View(@event);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Event? @event)
    {
        if (@event == null) return NotFound();
        if (!ModelState.IsValid) return View();
       
        @event.UpdatedDate = DateTime.Now;
        context.Update(@event);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var @event = await context.Events.FindAsync(id);
        if (@event == null) return NotFound();
        return View(@event);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Event? @event)
    {
        if (@event == null) return NotFound();
        context.Remove(@event);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}