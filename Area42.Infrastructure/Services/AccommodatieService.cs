using System.Collections.Generic;
using System.Threading.Tasks;
using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Area42.Infrastructure.Data;

namespace Area42.Infrastructure.Services
{
    public class AccommodatieService : IAccommodatieService
    {
        private readonly IAccommodatieRepository _repo;

        public AccommodatieService(IAccommodatieRepository repo)
        {
            _repo = repo;
        }

        public Task<List<Accommodatie>> GetAllAccommodatiesAsync()
            => _repo.GetAllAccommodatiesAsync();

        public Task AddAccommodatieAsync(Accommodatie accommodatie)
            => _repo.AddAccommodatieAsync(accommodatie);

        public Task DeleteAccommodatieAsync(int id)
            => _repo.DeleteAccommodatieAsync(id);
    }

}