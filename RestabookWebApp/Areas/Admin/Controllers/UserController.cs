using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestabookWebApp.Areas.Admin.ViewModels;
using RestabookWebApp.Models;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
public class UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) : Controller
{
    public IActionResult Index()
    {
        var users = userManager.Users.ToList();
        return View(users);
    }

    public async Task<IActionResult> AddRole(string? id)
    {
        if (id == null) return NotFound();

        var user = await userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var roles = await roleManager.Roles.Select(x => x.Name).ToListAsync();
        //userin sahib olduğu rolu ttapırıq
        var userRole = await userManager.GetRolesAsync(user);

        UserRoleVm model = new()
        {
            AppUser = user,
            Roles = roles.Except(userRole).ToList()
        };
        // Roles = roles.Where(role => !userRole.Contains(role)).ToList();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRole(string? id, string? role)
    {
        if (id == null || role == null) return NotFound("User ID or Role cannot be null");
        var user = await userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var result = await userManager.AddToRoleAsync(user, role);
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

    public async Task<IActionResult> Edit(string? userId)
    {
        if (userId == null) return NotFound();
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();
        return View(user);
    }
    
    public async Task<IActionResult> Delete(string? userId, string role)
    {
        if (userId == null) return NotFound();
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();
        await userManager.RemoveFromRoleAsync(user, role);

        return RedirectToAction("Index");
    }
}