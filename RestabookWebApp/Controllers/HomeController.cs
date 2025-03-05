using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var categories = context.Categories
                .Include(x => x.Products).ToList();
            var products = context.Products
                .OrderByDescending(x => x.CreatedDate).ToList();

            HomeVM homeVm = new()
            {
                Sliders = sliders,
                About = about,
                Services = services,
                Chefs = chefs,
                Events = events,
                Products = products,
                Categories = categories
            };
            return View(homeVm);
        }
    }
}