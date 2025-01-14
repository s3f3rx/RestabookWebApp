using Microsoft.AspNetCore.Mvc;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
public class DashboardController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}