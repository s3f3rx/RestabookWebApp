using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestabookWebApp.Data;
using RestabookWebApp.Models;
using RestabookWebApp.Services;

namespace RestabookWebApp.Controllers;

public class ReservationController(AppDbContext context, IEmailManager emailManager) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reservation(Reservation? reservation)
    {
        if (reservation == null)
            return NotFound();

        

        var existingReservation = await context.Reservations
            .FirstOrDefaultAsync(x => x.ResDate == reservation.ResDate
                                      && x.ResTime == reservation.ResTime
                                      && x.Email == reservation.Email
                                      && x.Phone == reservation.Phone);

        if (existingReservation != null)
        {
            ModelState.AddModelError(string.Empty, "A reservation already exists for this date and time.");
            return View("Index");
        }

        reservation.CreatedDate = DateTime.Now;
        context.Reservations.Add(reservation);
        await context.SaveChangesAsync();

        const string subject = "Restabook Reservation Information";

        var body = $@"
        <h3>Dear {reservation.Name},</h3>
        <p>Your reservation has been successfully made!</p>
        <p><strong>Date:</strong> {reservation.ResDate}</p>
        <p><strong>Time:</strong> {reservation.ResTime}</p>
        <p><strong>Number of People:</strong> {reservation.Persons}</p>";

        const string text = "Thank you for your reservation. We look forward to serving you!";

        emailManager.SendEmail(reservation.Email, subject, body, text);

        ViewBag.SuccessMessage = "Your reservation has been successfully made!";
        return RedirectToAction("Index");
    }
}