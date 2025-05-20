using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Area42.WebUI.Pages.Accommodaties
{
    public class IndexModel : PageModel
    {
        private readonly IAccommodatieService _accommodatieService;

        public IndexModel(IAccommodatieService accommodatieService)
        {
            _accommodatieService = accommodatieService;
        }

        // Zorg dat deze property niet null blijft; initializeer hem met een lege lijst.
        public List<Accommodatie> Accommodaties { get; set; } = new List<Accommodatie>();

        public async Task OnGetAsync()
        {
            // Vul de lijst met accommodaties uit de database
            Accommodaties = await _accommodatieService.GetAllAccommodatiesAsync();
        }
    }
}