using Area42.Application.Interfaces;
using Area42.Infrastructure.Data;
using Area42.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

//razer pages
builder.Services.AddRazorPages();

//repository's
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAccommodatieRepository, AccommodatieRepository>();
builder.Services.AddScoped<IReserveringRepository, ReserveringRepository>();

//services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccommodatieService, AccommodatieService>();
builder.Services.AddScoped<IReserveringService, ReserveringService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

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