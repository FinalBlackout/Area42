using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        
        public List<Reservering> Reserveringen { get; set; } = new List<Reservering>();

        public async Task<IActionResult> OnPostUpdateStatusAsync(int id, string actionType)
        {
            System.Diagnostics.Debug.WriteLine($"OnPostUpdateStatusAsync aangeroepen voor id {id} met actie {actionType}");

            if (actionType == "approve")
            {
                await _reserveringService.ApproveReserveringAsync(id);
                TempData["SuccessMessage"] = "Reservering is goedgekeurd.";
            }
            else if (actionType == "cancel")
            {
                await _reserveringService.RejectReserveringAsync(id);
                TempData["SuccessMessage"] = "Reservering is geannuleerd.";
            }
            else if (actionType == "delete")
            {
                await _reserveringService.DeleteReserveringAsync(id);
                TempData["SuccessMessage"] = "Reservering is verwijderd.";
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Onbekende actie.");
                return Page();
            }
            return RedirectToPage();
        }


        public async Task OnGetAsync()
        {
            string userId = null;
            if (!User.IsInRole("Medewerker"))
            {
                // Haal de user-ID op uit de NameIdentifier claim
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            Reserveringen = await _reserveringService.GetReserveringenAsync(userId);
        }
    }
}