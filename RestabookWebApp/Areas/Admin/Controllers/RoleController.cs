using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
[Authorize]
public class RoleController(RoleManager<IdentityRole> roleManager) : Controller
{
    public IActionResult Index()
    {
        var roles = roleManager.Roles.ToList();
        return View(roles);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IdentityRole? role)
    {
        if (role == null) return NotFound();
        var result = await roleManager.CreateAsync(role);
        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View();
    }

    public async Task<IActionResult> Edit(string? id) //roleId yazanda işləmir
    {
        if (id == null) return NotFound();
        var role = await roleManager.FindByIdAsync(id);
        if (role == null)
            return NotFound();

        return View(role);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(IdentityRole? role)
    {
        if (role == null) return NotFound();
        var existingRole = await roleManager.FindByIdAsync(role.Id);
        if (existingRole == null) return NotFound();

        // köhnə adı yeni ad ilə dəyişirik
        existingRole.Name = role.Name;
        var result = await roleManager.UpdateAsync(existingRole);

        if (result.Succeeded)
        {
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(existingRole);
    }

    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null) return NotFound();
        var role = await roleManager.FindByIdAsync(id);

        if (role == null)
            return NotFound();

        return View(role);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(IdentityRole? identityRole)
    {
        if (identityRole == null) return NotFound();
        var role = await roleManager.FindByIdAsync(identityRole.Id);

        if (role == null)
            return NotFound();

        var result = await roleManager.DeleteAsync(role);
        if (result.Succeeded) return RedirectToAction("Index");

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View("Delete", identityRole);
    }
}