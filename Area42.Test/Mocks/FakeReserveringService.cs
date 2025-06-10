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
        // In-memory opslag van reserveringen
        private readonly List<Reservering> _reserveringen = new List<Reservering>();

        // Haalt alle reserveringen op.
        public Task<List<Reservering>> GetAllReserveringenAsync()
        {
            return Task.FromResult(_reserveringen);
        }

        // Haalt reserveringen op voor een gebruiker (in deze fake maakt dat geen echt verschil)
        public Task<List<Reservering>> GetReserveringenVoorUserAsync(ClaimsPrincipal user)
        {
            // Voor deze fake returnen we gewoon alle reserveringen
            return Task.FromResult(_reserveringen);
        }

        // Voegt een nieuwe reservering toe.
        public Task CreateReserveringAsync(Reservering reservering)
        {
            // Hierbij gaan we er vanuit dat de reservering een unique Id heeft toegekend.
            _reserveringen.Add(reservering);
            return Task.CompletedTask;
        }

        // Update een bestaande reservering.
        public Task UpdateReserveringAsync(Reservering reservering)
        {
            var bestaandeReservering = _reserveringen.FirstOrDefault(r => r.Id == reservering.Id);
            if (bestaandeReservering != null)
            {
                // Update de properties. Hier pas je aan wat er allemaal moet.
                bestaandeReservering.Status = reservering.Status;
                bestaandeReservering.Startdatum = reservering.Startdatum;
                bestaandeReservering.Einddatum = reservering.Einddatum;
                // Voeg eventueel extra properties toe
            }
            return Task.CompletedTask;
        }

        // Verwijdert een reservering op basis van reserverings-ID.
        public Task DeleteReserveringAsync(int reserveringId)
        {
            _reserveringen.RemoveAll(r => r.Id == reserveringId);
            return Task.CompletedTask;
        }

        // Goedkeurt een reservering door de status aan te passen naar "Goedgekeurd".
        public Task ApproveReserveringAsync(int reserveringId)
        {
            var reservering = _reserveringen.FirstOrDefault(r => r.Id == reserveringId);
            if (reservering != null)
            {
                reservering.Status = "Goedgekeurd";
            }
            return Task.CompletedTask;
        }

        // Wijzigt de status van een reservering naar "Geweigerd"
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