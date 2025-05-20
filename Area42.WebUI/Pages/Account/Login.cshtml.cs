using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Area42.WebUI.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Eventuele initiële logica
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Gelieve uw gebruikersnaam en wachtwoord in te vullen.";
                return Page();
            }

            // Haal de connection string uit appsettings.json
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Variabelen om de opgehaalde database-gegevens op te slaan
            string dbUsername = null;
            string dbPassword = null;
            string dbRole = null;

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Zoek de gebruiker op basis van de (lowercase) gebruikersnaam
                    string query = "SELECT Username, Password, Role FROM Users WHERE LOWER(Username) = @Username LIMIT 1";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", Username.ToLower());
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                dbUsername = reader.GetString("Username");
                                dbPassword = reader.GetString("Password");
                                dbRole = reader.GetString("Role");
                            }
                        }
                    }
                }

                // Geen gebruiker gevonden? Geef een foutmelding.
                if (string.IsNullOrEmpty(dbUsername))
                {
                    ErrorMessage = "Ongeldige gebruikersnaam of wachtwoord.";
                    return Page();
                }

                // Vergelijk wachtwoorden (in productie: controleer met gehashte waarden)
                if (Password != dbPassword)
                {
                    ErrorMessage = "Ongeldige gebruikersnaam of wachtwoord.";
                    return Page();
                }

                // Maak de benodigde claims aan op basis van de opgehaalde gegevens.
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dbUsername),
                    new Claim(ClaimTypes.Role, dbRole)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Meld de gebruiker aan via de cookie-authenticatie
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                // In productie zou je dit loggen
                ErrorMessage = "Er is een fout opgetreden tijdens het inloggen: " + ex.Message;
                return Page();
            }
        }
    }
}