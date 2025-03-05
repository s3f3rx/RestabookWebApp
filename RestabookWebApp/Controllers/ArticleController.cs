using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestabookWebApp.Data;

namespace RestabookWebApp.Controllers;

public class ArticleController(AppDbContext context) : Controller
{
    public IActionResult Index(int page = 1)
    {
        const int pageSize = 2;

        var articles = context.Articles
            .Include(x => x.AppUser)
            .Include(x => x.Category)
            .Include(x => x.ArticleTags)
            .ThenInclude(x => x.Tag)
            .Where(x => x.IsDeleted == false && x.IsActive == true)
            .OrderByDescending(x => x.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var totalCount = context.Articles
            .Count(x => x.IsDeleted == false && x.IsActive == true);

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        var tags = context.Tags.ToList();
        ViewBag.Tags = tags;
        var categories = context.Categories.ToList();
        ViewBag.Categories = categories;

        return View(articles);
    }

    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null) return NotFound();

        var article = await context.Articles
            .Include(x => x.AppUser)
            .Include(x => x.Category)
            .Include(x => x.ArticleTags)
            .ThenInclude(x => x.Tag)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (article == null) return NotFound();

        var recentArticles = await context.Articles
            .Where(x => x.Id != article.Id)
            .OrderByDescending(x => x.CreatedDate).Take(3).ToListAsync();
        ViewBag.RecentArticles = recentArticles;

        var relatedArticles = await context.Articles
            .Where(x => x.CategoryId == article.CategoryId && x.Id != article.Id && x.IsActive == true &&
                        x.IsDeleted == false)
            .OrderByDescending(x => x.CreatedDate).Take(3).ToListAsync();
        ViewBag.RelatedArticles = relatedArticles;

        var tags = await context.Tags.ToListAsync();
        ViewBag.Tags = tags;

        var categories = await context.Categories.ToListAsync();
        ViewBag.Categories = categories;
        return View(article);
    }
}