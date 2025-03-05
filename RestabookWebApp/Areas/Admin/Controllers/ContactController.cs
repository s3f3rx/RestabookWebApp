using Microsoft.AspNetCore.Mvc;
using RestabookWebApp.Data;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
public class ContactController(AppDbContext context) : Controller
{
    public IActionResult Index()
    {
        var contacts = context.Contacts.ToList();
        return View(contacts);
    }
}