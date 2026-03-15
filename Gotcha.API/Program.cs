using System.Text.Json.Serialization;
using Gotcha.Core.Data;
using Gotcha.Core.Data.Seeder;
using Gotcha.Core.Services;
using Gotcha.Core.Services.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();

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

// CORS — allow all for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Seed the database
using (IServiceScope scope = app.Services.CreateScope())
{
    GotchaDbContext context = scope.ServiceProvider.GetRequiredService<GotchaDbContext>();
    await Seeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
