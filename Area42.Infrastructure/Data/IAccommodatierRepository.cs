using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Area42.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Area42.Infrastructure.Data
{
    public interface IAccommodatieRepository
    {
        Task<List<Accommodatie>> GetAllAccommodatiesAsync();
        Task AddAccommodatieAsync(Accommodatie accommodatie);
        Task DeleteAccommodatieAsync(int id);
    }
}