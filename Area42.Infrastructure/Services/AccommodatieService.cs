using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Area42.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Area42.Infrastructure.Services
{
    public class AccommodatieService : IAccommodatieService
    {
        private readonly IAccommodatieRepository _repository;

        public AccommodatieService(IAccommodatieRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Accommodatie>> GetAllAccommodatiesAsync()
        {
            return await _repository.GetAllAccommodatiesAsync();
        }

        // Andere methoden voor create, update, delete kunnen hier later toegevoegd worden.
    }
}