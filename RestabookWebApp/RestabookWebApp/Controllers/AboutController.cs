using Microsoft.AspNetCore.Mvc;
using RestabookWebApp.Data;
using RestabookWebApp.ViewModels;

namespace RestabookWebApp.Controllers;

public class AboutController(AppDbContext context) : Controller
{
    // GET
    public IActionResult Index()
    {
        var about = context.Abouts.FirstOrDefault();
        var chefs = context.Chefs
            .OrderByDescending(x => x.CreatedDate).ToList();
        AboutVm aboutVm = new()
        {
            About = about,
            Chefs = chefs
        };
        return View(aboutVm);
    }
}