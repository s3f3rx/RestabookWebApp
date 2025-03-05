using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestabookWebApp.DTOs;
using RestabookWebApp.Models;
using RestabookWebApp.Services;

namespace RestabookWebApp.Controllers;

public class AuthController(
    UserManager<AppUser> userManager,
    IEmailManager emailManager,
    SignInManager<AppUser> signInManager) : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDTO login)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var user = await userManager.FindByEmailAsync(login.Email);
        if (user == null)
        {
            ModelState.AddModelError("error", "Email or Password is not correct!");
            return View();
        }

        if (!user.EmailConfirmed)
        {
            ModelState.AddModelError("error", "Email is not confirmed!");
            return View();
        }

        var result = await signInManager.PasswordSignInAsync(user, login.Password,
            login.RememberMe, true);

        if (result.Succeeded)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError("error", "Email or Password is not correct!");
        return View();
    }


    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDTO register)
    {
        if (!ModelState.IsValid)
        {
            return View(register);
        }

        AppUser appUser = new()
        {
            FirstName = register.FirstName,
            LastName = register.LastName,
            Email = register.Email,
            UserName = register.Email,
            EmailConfirmed = false
        };
        var result = await userManager.CreateAsync(appUser, register.Password);

        if (register.Password != register.ConfirmPassword)
            ModelState.AddModelError("Password", "Password and Confirm Password do not match.");

        if (result.Succeeded)
        {
            var gentoken = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var link = Url.Action("Confirm", "Auth",
                new
                {
                    userId = appUser.Id,
                    token = gentoken
                },
                protocol: HttpContext.Request.Scheme);

            const string confirmText = "Thank you for registering. To confirm your email, click the link below:";
            if (link != null)
                emailManager.SendEmail(appUser.Email, link, "Confirm Email", confirmText);

            // ViewBag.EmailConfirmationMessage = "Please check your email to confirm your account.";
            // return View();
            return RedirectToAction("Login", "Auth");
        }


        return View();
    }

    public async Task<IActionResult> Confirm(string userid, string token)
    {
        var user = await userManager.FindByIdAsync(userid);
        if (user == null) return NotFound();

        var result = await userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            // TempData["EmailConfirmedMessage"] = "Your email has been confirmed. You can now log in.";
            // return RedirectToAction("Login", "Auth");
            return View();
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("error", error.Description);
        }

        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPassword)
    {
        if (!ModelState.IsValid)
            return View();

        var user = await userManager.FindByEmailAsync(forgotPassword.Email);
        if (user == null)
        {
            ModelState.AddModelError("Email", "No user found with this email address.");
            return View();
        }

        //reset tokeni
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

        //reset səhifəsinin urli
        var resetUrl = Url.Action("ResetPassword", "Auth",
            new
            {
                token = resetToken,
                email = forgotPassword.Email
            }, protocol: HttpContext.Request.Scheme);

        const string resetText = "You requested a password reset. Click the link below to reset your password:";
        if (resetUrl != null)
            emailManager.SendEmail(forgotPassword.Email, resetUrl, "Reset Password", resetText);

        ViewBag.Message = "A password reset link has been sent to your email address.";
        return View();
    }

    public IActionResult ResetPassword(string? token, string? email)
    {
        if (token == null || email == null)
            return BadRequest("Invalid token or email provided.");

        //urldən gələn email və tokeni bərabərləşdiririk
        var resetPasswordDto = new ResetPasswordDTO
        {
            Token = token,
            Email = email
        };
        return View(resetPasswordDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPassword)
    {
        if (!ModelState.IsValid)
            return View();

        if (resetPassword.NewPassword != resetPassword.ConfirmPassword)
        {
            ModelState.AddModelError("ConfirmPassword", "New Password and Confirm Password do not match.");
            return View();
        }

        var user = await userManager.FindByEmailAsync(resetPassword.Email);
        if (user == null)
        {
            ModelState.AddModelError("usererror", "User not found.");
            return View();
        }

        var result = await userManager
            .ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
        if (result.Succeeded)
        {
            ViewBag.Message = "Your password has been successfully reset. You can now log in.";
            return RedirectToAction("Login", "Auth");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("err", error.Description);
        }

        return View();
    }

    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePassword)
    {
        if (!ModelState.IsValid) return View(changePassword);

        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Auth");

        if (changePassword.NewPassword != changePassword.ConfirmNewPassword)
        {
            ModelState.AddModelError("ConfirmNewPassword", "New Password and Confirm New Password do not match.");
            return View(changePassword);
        }

        //şifrə dəyişdirmə metodu
        var result = await userManager
            .ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);

        if (result.Succeeded)
        {
            //şifrə dəyişdikdən sonra istifadəçini yenidən sign in edir
            await signInManager.RefreshSignInAsync(user);
            ViewBag.Message = "Your password has been successfully changed.";
            return View(changePassword);
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(changePassword);
    }
}