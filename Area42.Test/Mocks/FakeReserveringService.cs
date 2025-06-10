using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Area42.Application.Interfaces;
using Area42.Domain.Entities;

namespace Area42.Test.Mocks
{
    public class FakeReserveringService : IReserveringService
    {
        private readonly List<Reservering> _reserveringen = new List<Reservering>();

        public Task<List<Reservering>> GetAllReserveringenAsync()
        {
            return Task.FromResult(_reserveringen);
        }

        public Task<List<Reservering>> GetReserveringenVoorUserAsync(ClaimsPrincipal user)
        {
            return Task.FromResult(_reserveringen);
        }

        public Task CreateReserveringAsync(Reservering reservering)
        {
            _reserveringen.Add(reservering);
            return Task.CompletedTask;
        }

        public Task UpdateReserveringAsync(Reservering reservering)
        {
            var bestaandeReservering = _reserveringen.FirstOrDefault(r => r.Id == reservering.Id);
            if (bestaandeReservering != null)
            {
                bestaandeReservering.Status = reservering.Status;
                bestaandeReservering.Startdatum = reservering.Startdatum;
                bestaandeReservering.Einddatum = reservering.Einddatum;
            }
            return Task.CompletedTask;
        }

        public Task DeleteReserveringAsync(int reserveringId)
        {
            _reserveringen.RemoveAll(r => r.Id == reserveringId);
            return Task.CompletedTask;
        }

        public Task ApproveReserveringAsync(int reserveringId)
        {
            var reservering = _reserveringen.FirstOrDefault(r => r.Id == reserveringId);
            if (reservering != null)
            {
                reservering.Status = "Goedgekeurd";
            }
            return Task.CompletedTask;
        }

        public Task RejectReserveringAsync(int reserveringId)
        {
            var reservering = _reserveringen.FirstOrDefault(r => r.Id == reserveringId);
            if (reservering != null)
            {
                reservering.Status = "Geweigerd";
            }
            return Task.CompletedTask;
        }
    }
}