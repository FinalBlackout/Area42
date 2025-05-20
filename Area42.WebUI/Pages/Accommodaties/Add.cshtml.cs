using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Area42.WebUI.Pages.Accommodaties
{
    [Authorize(Roles = "Medewerker")]
    public class AddModel : PageModel
    {
        private readonly IAccommodatieService _accommodatieService;
        private readonly IWebHostEnvironment _environment;

        public AddModel(IAccommodatieService accommodatieService, IWebHostEnvironment environment)
        {
            _accommodatieService = accommodatieService;
            _environment = environment;
        }

        
        [BindProperty]
        public Accommodatie Accommodatie { get; set; } = new Accommodatie();


        [BindProperty, Required(ErrorMessage = "Kies een afbeelding.")]
        public IFormFile ImageFile { get; set; }

        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            // Eventuele initiële logica voor de pagina
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Kies een afbeelding.");
                return Page();
            }

            // Optionele: valideer het bestandstype zoals je eerder deed
            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var ext = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || Array.IndexOf(permittedExtensions, ext) < 0)
            {
                ModelState.AddModelError("ImageFile", "Ongeldig bestandstype. Alleen .jpg, .jpeg, .png en .gif zijn toegestaan.");
                return Page();
            }

            // Bestandsopslag logica
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "accommodaties");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var uniqueFileName = Guid.NewGuid().ToString() + ext;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(fileStream);
            }
            Accommodatie.ImagePath = Path.Combine("images", "accommodaties", uniqueFileName).Replace("\\", "/");

            // Je overige logica voor opslag in de database...
            await _accommodatieService.AddAccommodatieAsync(Accommodatie);

            SuccessMessage = "Accommodatie en afbeelding succesvol toegevoegd.";
            ModelState.Clear();
            return Page();
        }
    }
}