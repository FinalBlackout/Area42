using System.Collections.Generic;
using System.Threading.Tasks;
using Area42.Domain.Entities;

namespace Area42.Infrastructure.Data
{
    public interface IReserveringRepository
    {
        Task<List<Reservering>> GetReserveringenAsync(string userId = null);
        Task<Reservering> GetReserveringByIdAsync(int id);
        Task AddAsync(Reservering reservering);
        Task UpdateAsync(Reservering reservering);
        Task DeleteAsync(int id);
        Task ApproveAsync(int id);
        Task RejectAsync(int id);
    }
}