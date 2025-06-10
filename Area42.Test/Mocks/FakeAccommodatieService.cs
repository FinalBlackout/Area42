using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Area42.Application.Interfaces;
using Area42.Domain.Entities;

namespace Area42.Test.Mocks
{
    public class FakeAccommodatieService : IAccommodatieService
    {
        // In-memory opslag voor accommodaties
        private readonly List<Accommodatie> _accommodaties = new List<Accommodatie>();

        public Task<List<Accommodatie>> GetAllAccommodatiesAsync()
        {
            return Task.FromResult(_accommodaties);
        }

        public Task AddAccommodatieAsync(Accommodatie accommodatie)
        {
            _accommodaties.Add(accommodatie);
            return Task.CompletedTask;
        }



        public Task DeleteAccommodatieAsync(int accommodatieId)
        {
            _accommodaties.RemoveAll(a => a.Id == accommodatieId);
            return Task.CompletedTask;
        }





    }
}