using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Area42.Infrastructure.Services;
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
    [Authorize]
    public class AddModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IReserveringService _reserveringService;
        private readonly IAccommodatieService _accommodatieService;

        public AddModel(IReserveringService reserveringService, IAccommodatieService accommodatieService)
        {
            _reserveringService = reserveringService;
            _accommodatieService = accommodatieService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Selecteer een accommodatie.")]
        public int AccommodatieId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Voer een startdatum in")]
        [DataType(DataType.Date)]
        public DateTime Startdatum { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Voer een einddatum in")]
        [DataType(DataType.Date)]
        public DateTime Einddatum { get; set; }

        public List<SelectListItem> AccommodatieSelectList { get; set; } = new List<SelectListItem>();

        public string SuccessMessage { get; set; }

        public async Task OnGetAsync()
        {
            await LoadAccommodatiesAsync();
        }

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

                System.Diagnostics.Debug.WriteLine($"Logged in user: {User.Identity.Name} with dummy userId: {userId}");
            }

            if (userId == 0)
            {
                ModelState.AddModelError(string.Empty, "Gebruiker kon niet worden gevonden in de database.");
                await LoadAccommodatiesAsync();
                return Page();
            }

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

            ModelState.Clear();
            await LoadAccommodatiesAsync();
            return Page();
        }
    }
}