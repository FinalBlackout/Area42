using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Area42.WebUI.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        // Dummy gebruikers: in productie vervang je dit met databasegegevens.
        private readonly Dictionary<string, (string Password, string Role)> _dummyUsers =
            new()
            {
                { "medewerker", ("medewerkerpass", "Medewerker") },
                { "gast", ("gastpass", "Klant") }
            };

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "ModelState is niet geldig.";
                return Page();
            }

            if (_dummyUsers.TryGetValue(Username.ToLower(), out var user) && user.Password == Password)
            {
                // Log eventueel even:  
                System.Diagnostics.Debug.WriteLine("Inloggen gelukt voor gebruiker: " + Username);

                // Verifieer dat de juiste claims worden aangemaakt
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Ongeldige gebruikersnaam of wachtwoord.";
                // Log de fout voor debugging:
                System.Diagnostics.Debug.WriteLine("Inlog poging mislukt voor: " + Username);
                return Page();
            }
        }
    }
}