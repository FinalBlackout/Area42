using Area42.Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Area42.Application.Interfaces
{
    public interface IReserveringService
    {
        Task<List<Reservering>> GetReserveringenVoorUserAsync(ClaimsPrincipal user);

        Task CreateReserveringAsync(Reservering reservering);

        Task<List<Reservering>> GetAllReserveringenAsync();

        Task UpdateReserveringAsync(Reservering reservering);

        Task DeleteReserveringAsync(int reserveringId);

        Task ApproveReserveringAsync(int reserveringId);

        Task RejectReserveringAsync(int reserveringId);
    }
}