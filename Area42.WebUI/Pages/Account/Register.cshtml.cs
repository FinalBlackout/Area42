using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Area42.WebUI.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public RegisterModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        [Required(ErrorMessage = "Voer uw gebruikersnaam in.")]
        public string Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Voer een wachtwoord in.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Bevestig uw wachtwoord.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Selecteer een type account.")]
        public string Role { get; set; }  // "Klant" of "Medewerker"

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            // Eventuele initiële logica
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Controleer uw gegevens en probeer het opnieuw.";
                return Page();
            }

            // Verkrijg de connection string uit appsettings.json
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Voor veiligheid: zorg in productie voor gehashte wachtwoorden
                    string query = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", Username);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Role", Role);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                SuccessMessage = "Registratie succesvol. U kunt nu inloggen.";
            }
            catch (MySqlException ex)
            {
                // Hier kun je de fout detailleren en eventueel loggen
                ErrorMessage = "Er is een fout opgetreden tijdens registratie: " + ex.Message;
            }

            return Page();
        }
    }
}