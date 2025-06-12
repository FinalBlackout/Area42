using System.Collections.Generic;
using System.Threading.Tasks;
using Area42.Domain.Entities;

namespace Area42.Application.Interfaces
{
    public interface IReserveringService
    {
        /// <summary>
        /// Haal alle reserveringen op als userId==null, anders alleen die van de user.
        /// </summary>
        Task<List<Reservering>> GetReserveringenAsync(string userId = null);

        /// <summary>
        /// Haal één reservering op op basis van ID.
        /// </summary>
        Task<Reservering> GetReserveringByIdAsync(int id);

        Task CreateReserveringAsync(Reservering reservering);

        Task UpdateReserveringAsync(Reservering reservering);

        Task DeleteReserveringAsync(int reserveringId);

        Task ApproveReserveringAsync(int reserveringId);

        Task RejectReserveringAsync(int reserveringId);
    }
}