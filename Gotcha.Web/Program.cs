using DotNetEnv;
using Gotcha.Core.Data;
using Gotcha.Core.Entities.Models;
using Gotcha.Core.Interfaces;
using Gotcha.Core.Services;
using Gotcha.Core.Services.Email;
using Gotcha.Core.Services.Payment;
using Gotcha.Core.Services.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// Load secrets from a .env file (walks up the directory tree from the working dir).
// Values land in Environment.GetEnvironmentVariables() and are picked up by the default
// AddEnvironmentVariables() that WebApplication.CreateBuilder registers — so they flow
// into IConfiguration the same way appsettings values do.
// Nested config keys use double underscore in env var names: PayPal__Secret → PayPal:Secret.
Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true; // prevents client-side JS from accessing the session cookie
    options.Cookie.IsEssential = true; // ensures the cookie is never sent over plain HTTP
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //session works regardless of cookie consent (appropriate since the session is for security logging, not tracking)
});

// Database
builder.Services.AddDbContext<GotchaDbContext>(
    options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("GotchaDbContext"))
);

// Repository Services
builder.Services.AddScoped<AttackerRepoService>();
builder.Services.AddScoped<GameRepoService>();
builder.Services.AddScoped<KillRepoService>();
builder.Services.AddScoped<LogRepoService>();
builder.Services.AddScoped<PlayerRepoService>();
builder.Services.AddScoped<RulesRepoService>();
builder.Services.AddScoped<UserRepoService>();

// Business Logic Services
builder.Services.AddScoped<GameService>();

// Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

// PayPal
builder.Services.Configure<PayPalSettings>(builder.Configuration.GetSection("PayPal"));
builder.Services.AddHttpClient<IPayPalService, PayPalService>();

// Registreer Identity services in de DI-container.
// AddIdentity<TUser, TRole> configureert:
// - UserManager: gebruikersbeheer (aanmaken, zoeken, verwijderen)
// - SignInManager: in-en uitloggen
// - RoleManager: rollenbeheer
// - Cookie-authenticatie schemas
builder.Services.AddIdentity<GotchaUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequiredLength = 12;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredUniqueChars = 4;

    options.User.RequireUniqueEmail = true;
})
// Koppelt Identity aan je database via Entity Framework
.AddEntityFrameworkStores<GotchaDbContext>()
// Voegt token providers toe voor wachtwoordreset, e-mailbevestiging, 2FA, ...
.AddDefaultTokenProviders();

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

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "user",
    areaName: "User",
    pattern: "User/{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "player",
    areaName: "Player",
    pattern: "Player/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
