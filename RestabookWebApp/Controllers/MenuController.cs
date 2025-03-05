using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestabookWebApp.Data;
using RestabookWebApp.ViewModels;

namespace RestabookWebApp.Controllers;

public class MenuController(AppDbContext context) : Controller
{
    public IActionResult Index()
    {
        var categories = context.Categories
            .Include(x => x.Products).ToList();
        var products = context.Products
            .OrderByDescending(x => x.CreatedDate).ToList();
        MenuVm menuVm = new()
        {
            Categories = categories,
            Products = products
        };
        return View(menuVm);
    }

    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null) return NotFound();

        var product = await context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null) return NotFound();

        var categories = await context.Categories.ToListAsync();

        var popularCategories = await context.Categories
            //hansı categoryə sahib productdan çoxdursa o categoryni tapır 
            .OrderByDescending(x => x.Products.Count)
            .Take(3)
            .ToListAsync();

        var popularProducts = await context.Products
            //productumun categorysi popularCategories içində varsa onu tapıb gətirir  
            .Where(x => popularCategories.Select(c => c.Id).Contains(x.CategoryId))
            .Include(x => x.Category)
            .ToListAsync();

        var relatedProducts = await context.Products
            //gətirdiyim productla eyni categorydən olan amma indi tapdığın productdan başqa productları gətir 
            .Where(x => x.CategoryId == product.CategoryId && x.Id != product.Id)
            .Include(x => x.Category)
            .ToListAsync();

        var detailVm = new DetailVm
        {
            Product = product,
            Categories = categories,
            PopularCategories = popularCategories,
            PopularProducts = popularProducts,
            RelatedProducts = relatedProducts
        };

        return View(detailVm);
    }
}