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
        public List<Accommodatie> Accommodaties { get; set; } = new();

        public IndexModel(IAccommodatieService accommodatieService)
        {
            _accommodatieService = accommodatieService;
        }

        public async Task OnGetAsync()
        {
            Accommodaties = await _accommodatieService.GetAllAccommodatiesAsync();
        }
    }
}