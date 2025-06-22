using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Area42.Domain.Entities;
using Area42.Infrastructure.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Area42.Pages.Account.Medewerker
{
    public class LoginModel : PageModel
    {
        private readonly IMedewerkerRepository _employeeRepository;

        public LoginModel(IMedewerkerRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var medewerker = await _employeeRepository.GetMedewerkerByEmailAsync(Email);

            if (medewerker == null || medewerker.Password != Password)
            {
                ErrorMessage = "Ongeldige gebruikersnaam of wachtwoord.";
                return Page();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, medewerker.Naam),
                new Claim(ClaimTypes.Email, medewerker.Email),
                new Claim(ClaimTypes.Role, "Medewerker")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToPage("/Index");

        }
    }
}
