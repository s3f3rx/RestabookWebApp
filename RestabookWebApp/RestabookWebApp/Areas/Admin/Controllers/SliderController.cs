using Microsoft.AspNetCore.Mvc;
using RestabookWebApp.Data;
using RestabookWebApp.Helpers;
using RestabookWebApp.Models;

namespace RestabookWebApp.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class SliderController(AppDbContext context, IWebHostEnvironment env) : Controller
    {
        public IActionResult Index()
        {
            var sliders = context.Sliders.ToList();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider? slider, IFormFile? file)
        {
            if (file == null)
            {
                ModelState.AddModelError("error", "Image is not valid!");
                return View();
            }

            if (!ModelState.IsValid) return View(slider);
            if (slider == null) return NotFound();

            slider.PhotoPath = await file.FileSaverAsync(env.WebRootPath, "slider");
            slider.CreatedDate = DateTime.Now;
            await context.Sliders.AddAsync(slider);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var slider = await context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Slider? slider, IFormFile? file)
        {
            if (slider == null) return NotFound();

            if (!ModelState.IsValid)
                return View(slider);

            if (file != null)
                slider.PhotoPath = await file.FileSaverAsync(env.WebRootPath, "slider");
            else
                ModelState.AddModelError("error", "images are not found!");

            slider.UpdatedDate = DateTime.Now;
            context.Sliders.Update(slider);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var slider = await context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Slider? slider)
        {
            // System.IO.File.Delete(_env.WebRootPath + slider.PhotoPath);
            if (slider == null) return NotFound();

            var filePath = Path.Combine(env.WebRootPath, slider.PhotoPath.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            else
            {
                ModelState.AddModelError("error", "Image is not valid or does not exist!");
                return View(slider);
            }

            context.Sliders.Remove(slider);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}