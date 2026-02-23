using Gotcha.Core.Data;
using Gotcha.Core.Services;
using Gotcha.Core.Services.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
