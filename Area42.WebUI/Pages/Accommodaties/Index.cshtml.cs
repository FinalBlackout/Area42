using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
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
        public List<Accommodatie> Accommodaties { get; set; } = new List<Accommodatie>();

        public async Task OnGetAsync()
        {
            Accommodaties = await _accommodatieService.GetAllAccommodatiesAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _accommodatieService.DeleteAccommodatieAsync(id);
            return RedirectToPage();
        }

    }
}