using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestabookWebApp.Data;
using RestabookWebApp.Helpers;
using RestabookWebApp.Models;

namespace RestabookWebApp.Areas.Admin.Controllers;

[Area(nameof(Admin))]
[Authorize]
public class ArticleController(AppDbContext context, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    : Controller
{
    public IActionResult Index()
    {
        var articles = context.Articles
            .Include(x => x.Category)
            .Include(x => x.ArticleTags)
            .ThenInclude(x => x.Tag)
            .Include(x => x.AppUser)
            .Where(x => x.IsDeleted == false)
            .OrderByDescending(x => x.CreatedDate)
            .ToList();
        return View(articles);
    }

    public IActionResult Create()
    {
        var categories = context.Categories.ToList();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        var tags = context.Tags.ToList();
        ViewBag.Tags = new SelectList(tags, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Article? article, IFormFile? file, List<int>? tags)
    {
        if (article == null || file == null)
        {
            ModelState.AddModelError("", "Article and file information are missing!");
            return View(article);
        }

        if (httpContextAccessor.HttpContext == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var userClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userClaim == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        var userId = userClaim.Value;
        article.AppUserId = userId;
        article.CreatedDate = DateTime.Now;
        article.PhotoPath = await file.FileSaverAsync(env.WebRootPath, "article");
        await context.Articles.AddAsync(article);
        await context.SaveChangesAsync();

        if (tags != null && tags.Any())
        {
            foreach (var tagId in tags)
            {
                ArticleTag articleTag = new()
                {
                    ArticleId = article.Id,
                    TagId = tagId
                };
                await context.ArticleTags.AddAsync(articleTag);
            }
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    public IActionResult Edit(int? id)
    {
        var categories = context.Categories.ToList();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        var article = context.Articles.Find(id);
        if (article == null) return NotFound();

        var tags = context.Tags.ToList();
        ViewBag.Tags = new SelectList(tags, "Id", "Name");
        return View(article);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Article? article, IFormFile? file, List<int>? tags)
    {
        if (article == null)
        {
            ModelState.AddModelError("", "Article information are missing!");
            return View(article);
        }

        article.UpdatedDate = DateTime.Now;
        //köhnə şəkli silib yenisini yükləyir
        if (file != null)
        {
            if (!string.IsNullOrEmpty(article.PhotoPath))
            {
                var oldPhotoPath = Path.Combine(env.WebRootPath, article.PhotoPath.TrimStart('/'));
                if (System.IO.File.Exists(oldPhotoPath))
                {
                    System.IO.File.Delete(oldPhotoPath);
                }
            }

            article.PhotoPath = await file.FileSaverAsync(env.WebRootPath, "article");
        }
        else
            ModelState.AddModelError("error", "images are not found!");

        context.Update(article);
        await context.SaveChangesAsync();

        var existingTags = context.ArticleTags.Where(x => x.ArticleId == article.Id).ToList();
        context.ArticleTags.RemoveRange(existingTags);

        if (tags != null && tags.Any())
        {
            foreach (var tagId in tags)
            {
                ArticleTag articleTag = new()
                {
                    ArticleId = article.Id,
                    TagId = tagId
                };
                await context.ArticleTags.AddAsync(articleTag);
            }
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var article = await context.Articles.FindAsync(id);
        if (article == null) return NotFound();

        return View(article);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int? id, bool hardDelete = false)
    {
        if (id == null) return NotFound();
        var article = await context.Articles.FindAsync(id);
        if (article == null) return NotFound();

        //true olsa hard delete
        if (hardDelete)
        {
            var photoPath = Path.Combine(env.WebRootPath, article.PhotoPath.TrimStart('/'));
            if (System.IO.File.Exists(photoPath))
            {
                System.IO.File.Delete(photoPath);
            }

            context.Articles.Remove(article);
        }
        else //false olsa soft delete
        {
            article.IsDeleted = true;
            article.IsActive = false;
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}