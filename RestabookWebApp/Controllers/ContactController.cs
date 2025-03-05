using Microsoft.AspNetCore.Mvc;
using RestabookWebApp.Data;
using RestabookWebApp.Models;
using RestabookWebApp.Services;

namespace RestabookWebApp.Controllers;

public class ContactController(AppDbContext context, IEmailManager emailManager) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(Contact? contact)
    {
        if (!ModelState.IsValid || contact == null)
        {
            ModelState.AddModelError(string.Empty, "Please fill out all the fields");
            return View("Index");
        }

        contact.CreatedDate = DateTime.Now;
        context.Contacts.Add(contact);
        await context.SaveChangesAsync();

        const string subject = "Restabook Contact Information";
        var body = $@"
        <h3>Dear {contact.Name},</h3>
        <p>We have received your message and will get back to you as soon as possible.</p>
        <p><strong>Subject:</strong> {contact.Subject}</p>
        <p><strong>Your Message:</strong> {contact.Message}</p>";
        const string text = "Thank you for reaching out! Our team will contact you shortly.";

        emailManager.SendEmail(contact.Email, body, subject, text);

        ViewBag.SuccessMessage = "Your message has been sent successfully! We will get back to you soon.";
        return RedirectToAction("Index");
    }
}