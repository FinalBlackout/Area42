using System.Collections.Generic;
using System.Threading.Tasks;
using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Area42.Infrastructure.Data;

namespace Area42.Infrastructure.Services
{
    public class ReserveringService : IReserveringService
    {
        private readonly IReserveringRepository _repo;
        public ReserveringService(IReserveringRepository repo)
            => _repo = repo;

        public Task<List<Reservering>> GetReserveringenAsync(string userId = null)
            => _repo.GetReserveringenAsync(userId);

        public Task<Reservering> GetReserveringByIdAsync(int id)
            => _repo.GetReserveringByIdAsync(id);

        public Task CreateReserveringAsync(Reservering r)
            => _repo.AddAsync(r);

        public Task UpdateReserveringAsync(Reservering r)
            => _repo.UpdateAsync(r);

        public Task DeleteReserveringAsync(int id)
            => _repo.DeleteAsync(id);

        public Task ApproveReserveringAsync(int id)
            => _repo.ApproveAsync(id);

        public Task RejectReserveringAsync(int id)
            => _repo.RejectAsync(id);
    }
}