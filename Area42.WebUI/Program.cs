using Area42.Application;
using Area42.Application.Interfaces;
using Area42.Infrastructure.Data;
using Area42.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Voeg Razor Pages toe
builder.Services.AddRazorPages();

// Haal de connection string op uit appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registreer onze repository en service via DI
builder.Services.AddScoped<IAccommodatieRepository>(provider => new AccommodatieRepository(connectionString));
builder.Services.AddScoped<IAccommodatieService, AccommodatieService>();
builder.Services.AddScoped<IReserveringService, ReserveringService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Verwijs naar de loginpagina als niet-authenticated
        options.AccessDeniedPath = "/Account/AccessDenied"; // Optioneel voor afgewezen toegang
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();