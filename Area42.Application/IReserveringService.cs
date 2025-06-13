using System.Collections.Generic;
using System.Threading.Tasks;
using Area42.Domain.Entities;

namespace Area42.Application.Interfaces
{
    public interface IReserveringService
    {
        Task<List<Reservering>> GetReserveringenAsync(string userId = null);
        Task<Reservering> GetReserveringByIdAsync(int id);
        Task CreateReserveringAsync(Reservering reservering);
        Task UpdateReserveringAsync(Reservering reservering);
        Task DeleteReserveringAsync(int reserveringId);
        Task ApproveReserveringAsync(int reserveringId);
        Task RejectReserveringAsync(int reserveringId);
    }
}