using System.Threading.Tasks;
using Area42.Application;
using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Area42.Infrastructure.Data;

namespace Area42.Infrastructure.Services
{
    public class MedewerkerService : IMedewerkerService
    {
        private readonly IMedewerkerRepository _medewerkerRepository;

        public MedewerkerService(IMedewerkerRepository medewerkerRepository)
        {
            _medewerkerRepository = medewerkerRepository;
        }

        public Task<Medewerker?> GetMedewerkerByEmailAsync(string email)
        {
            return _medewerkerRepository.GetMedewerkerByEmailAsync(email);
        }
    }
}
