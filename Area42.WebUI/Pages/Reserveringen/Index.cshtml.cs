using Area42.Domain.Entities;
using Area42.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Area42.WebUI.Pages.Reserveringen
{
    public class IndexModel : PageModel
    {
        private readonly IAccommodatieService _accommodatieService;
        private readonly IReserveringService _reserveringService;

        public IndexModel(IAccommodatieService accommodatieService, IReserveringService reserveringService)
        {
            _accommodatieService = accommodatieService;
            _reserveringService = reserveringService;
        }

        // Zorg dat deze property bestaat en correct is gespeld!
        public List<Accommodatie> Accommodaties { get; set; } = new List<Accommodatie>();

        [BindProperty]
        public int AccommodatieId { get; set; }

        [BindProperty]
        public System.DateTime Startdatum { get; set; }

        [BindProperty]
        public System.DateTime Einddatum { get; set; }

        public string Bericht { get; set; }

        public async Task OnGetAsync()
        {
            // Vul de lijst met accommodaties
            Accommodaties = await _accommodatieService.GetAllAccommodatiesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Voorbeeldvalidatie
            if (Startdatum >= Einddatum)
            {
                ModelState.AddModelError(string.Empty, "Einddatum moet later zijn dan de startdatum.");
                await OnGetAsync();
                return Page();
            }

            var nieuweReservering = new Reservering
            {
                AccommodatieId = AccommodatieId,
                Startdatum = Startdatum,
                Einddatum = Einddatum,
                Status = "In behandeling"
            };

            await _reserveringService.CreateReserveringAsync(nieuweReservering);
            Bericht = "Reservering succesvol geplaatst!";

            // Je kunt eventueel de pagina opnieuw laden of redirecten
            return RedirectToPage("/Reserveringen/Index");
        }
    }
}