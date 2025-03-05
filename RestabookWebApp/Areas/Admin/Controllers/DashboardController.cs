using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
[Authorize]
public class DashboardController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}