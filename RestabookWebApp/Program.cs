using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestabookWebApp.Data;
using RestabookWebApp.Models;
using RestabookWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//database connection
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


//login register üçün Dependency injection
builder.Services.AddDefaultIdentity<AppUser>()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>();


//referance between interface and class
builder.Services.AddSingleton<IEmailManager, EmailManager>();


//password user configuration
builder.Services.Configure<IdentityOptions>(option =>
{
    option.User.RequireUniqueEmail = true;
    option.SignIn.RequireConfirmedEmail = true;
    option.Lockout.MaxFailedAccessAttempts = 5;
    option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    // option.Tokens.EmailConfirmationTokenProvider = "default";
    // option.Tokens.EmailConfirmationTokenLifespan = TimeSpan.FromDays(5);
});


//login olmasaq adminə keçid edə bilməyək
builder.Services.ConfigureApplicationCookie(option => { option.LoginPath = "/auth/login"; });


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();