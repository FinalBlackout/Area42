using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Area42.WebUI.Pages.Reserveringen
{
    [Authorize] // Zorg dat alleen ingelogde gebruikers deze pagina kunnen zien
    public class IndexModel : PageModel
    {
        private readonly IReserveringService _reserveringService;

        public IndexModel(IReserveringService reserveringService)
        {
            _reserveringService = reserveringService;
        }

        // Deze property vult de naam van de reserveringen (ofwel alle, of alleen de eigen reserveringen)
        public List<Reservering> Reserveringen { get; set; } = new List<Reservering>();

        public async Task OnGetAsync()
        {
            // De service controleert intern de rol van de ingelogde gebruiker.
            // Als de gebruiker "Medewerker" is wordt alle reserveringen teruggegeven.
            // Bij een "Klant" worden alleen de reserveringen van de klant opgehaald.
            Reserveringen = await _reserveringService.GetReserveringenVoorUserAsync(User);
        }
    }
}