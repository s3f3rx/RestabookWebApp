using Microsoft.AspNetCore.Mvc;
using RestabookWebApp.Data;
using RestabookWebApp.Models;
using RestabookWebApp.ViewModels;

namespace RestabookWebApp.Controllers
{
    public class HomeController(AppDbContext context) : Controller
    {
        public IActionResult Index()
        {
            var sliders = context.Sliders.ToList();
            var about = context.Abouts.FirstOrDefault();
            var services = context.Services
                .OrderByDescending(x => x.CreatedDate).Take(3).ToList();
            var chefs = context.Chefs
                .OrderByDescending(x => x.CreatedDate).ToList();
            var events = context.Events
                .OrderByDescending(x => x.CreatedDate).ToList();
            var products = context.Products.ToList();
            HomeVM homeVm = new()
            {
                Sliders = sliders,
                About = about,
                Services = services,
                Chefs = chefs,
                Events = events,
                Products = products
            };
            return View(homeVm);
        }
    }
}