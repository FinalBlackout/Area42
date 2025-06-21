using System.Threading.Tasks;
using Area42.Domain.Entities;

namespace Area42.Infrastructure.Data
{
    public interface IMedewerkerRepository
    {
        Task<Medewerker?> GetMedewerkerByEmailAsync(string username);
    }
}