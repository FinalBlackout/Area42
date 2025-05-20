using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Area42.WebUI.Pages.Reserveringen
{
    [Authorize] // Alleen ingelogde gebruikers kunnen reserveringen aanvragen.
    public class AddModel : PageModel
    {
        private readonly IReserveringService _reserveringService;
        private readonly IAccommodatieService _accommodatieService;

        public AddModel(IReserveringService reserveringService, IAccommodatieService accommodatieService)
        {
            _reserveringService = reserveringService;
            _accommodatieService = accommodatieService;
        }

        // Bind de gekozen accommodatie uit de dropdown.
        [BindProperty]
        [Required(ErrorMessage = "Selecteer een accommodatie.")]
        public int AccommodatieId { get; set; }

        // Startdatum met validatie.
        [BindProperty]
        [Required(ErrorMessage = "Voer een startdatum in")]
        [DataType(DataType.Date)]
        public DateTime Startdatum { get; set; }

        // Einddatum met validatie.
        [BindProperty]
        [Required(ErrorMessage = "Voer een einddatum in")]
        [DataType(DataType.Date)]
        public DateTime Einddatum { get; set; }

        // Deze property vullen we met de beschikbare accommodaties voor de dropdown.
        public List<SelectListItem> AccommodatieSelectList { get; set; } = new List<SelectListItem>();

        public string SuccessMessage { get; set; }

        public async Task OnGetAsync()
        {
            await LoadAccommodatiesAsync();
        }

        // Haalt alle accommodaties op via de service en vult de dropdown.
        private async Task LoadAccommodatiesAsync()
        {
            var accommodaties = await _accommodatieService.GetAllAccommodatiesAsync();
            AccommodatieSelectList.Clear();

            foreach (var acc in accommodaties)
            {
                AccommodatieSelectList.Add(new SelectListItem
                {
                    Value = acc.Id.ToString(),
                    Text = $"{acc.Naam} - {acc.Type}"
                });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadAccommodatiesAsync();
                return Page();
            }

            if (Startdatum >= Einddatum)
            {
                ModelState.AddModelError(string.Empty, "Einddatum moet na de startdatum liggen.");
                await LoadAccommodatiesAsync();
                return Page();
            }

            // Haal het user-id op via een dummy mapping op basis van de ingelogde gebruikersnaam.
            // In productie verwacht je dat de claim of via een lookup-service het juiste userId teruggeeft.
            int userId = 0;
            if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
            {
                string username = User.Identity.Name.ToLower();
                if (username == "gvanderwilligen")
                {
                    userId = 1;
                }
                else if (username == "bertha")
                {
                    userId = 2;
                }
                // Voeg hier extra logica toe indien er meer gebruikers zijn of haal de userId op via een service.
            }

            // Maak een nieuwe reservering aan met de ingevulde gegevens.
            var newReservation = new Reservering
            {
                AccommodatieId = this.AccommodatieId,
                UserId = userId,
                Startdatum = this.Startdatum,
                Einddatum = this.Einddatum,
                Status = "In behandeling"
            };

            await _reserveringService.CreateReserveringAsync(newReservation);

            SuccessMessage = "Reservering succesvol aangevraagd. Een medewerker zal uw aanvraag beoordelen.";

            // Wis de modelstate en herlaad de accommodatielijst zodat het formulier weer leeg is.
            ModelState.Clear();
            await LoadAccommodatiesAsync();
            return Page();
        }
    }
}