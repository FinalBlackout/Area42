using Area42.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Area42.Application.Interfaces
{
    public interface IAccommodatieService
    {
        Task<List<Accommodatie>> GetAllAccommodatiesAsync();
        // Andere methoden (bijv. GetById, Create, Update) kunnen later toegevoegd worden.
    }
}