using Area42.Application.Interfaces;
using Area42.Infrastructure.Data;
using Area42.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages
builder.Services.AddRazorPages();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMedewerkerRepository, MedewerkerRepository>();
builder.Services.AddScoped<IAccommodatieRepository, AccommodatieRepository>();
builder.Services.AddScoped<IReserveringRepository, ReserveringRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMedewerkerService, MedewerkerService>();
builder.Services.AddScoped<IAccommodatieService, AccommodatieService>();
builder.Services.AddScoped<IReserveringService, ReserveringService>();

// Authenticatie (voor zowel gebruikers als medewerkers geschikt)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// Middleware & Routing
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
