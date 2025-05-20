using Area42.Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Area42.Application.Interfaces
{
    public interface IReserveringService
    {
        /// <summary>
        /// Maakt een nieuwe reservering aan.
        /// </summary>
        /// <param name="reservering">De reservering die aangemaakt dient te worden.</param>
        Task CreateReserveringAsync(Reservering reservering);

        /// <summary>
        /// Haalt de reserveringen op voor de ingelogde gebruiker.
        /// Bij een klant worden enkel hun eigen reserveringen getoond,
        /// terwijl een medewerker alle reserveringen kan zien.
        /// </summary>
        /// <param name="user">De ClaimsPrincipal van de ingelogde gebruiker.</param>
        /// <returns>Lijst met reserveringen.</returns>
        Task<List<Reservering>> GetReserveringenVoorUserAsync(ClaimsPrincipal user);

        /// <summary>
        /// Haalt alle reserveringen op. Deze methode wordt doorgaans gebruikt door medewerkers.
        /// </summary>
        /// <returns>Lijst met alle reserveringen.</returns>
        Task<List<Reservering>> GetAllReserveringenAsync();

        /// <summary>
        /// Update een bestaande reservering.
        /// Deze methode is vooral bedoeld voor medewerkers, zodat zij wijzigingen kunnen doorvoeren (bijvoorbeeld het goedkeuren van een reservering).
        /// </summary>
        /// <param name="reservering">De gewijzigde reservering.</param>
        Task UpdateReserveringAsync(Reservering reservering);

        /// <summary>
        /// Verwijdert een reservering aan de hand van het reserverings-ID.
        /// </summary>
        /// <param name="reserveringId">Het unieke ID van de reservering.</param>
        Task DeleteReserveringAsync(int reserveringId);

        /// <summary>
        /// Keurt een reservering goed, zodat de status verandert naar "Bevestigd".
        /// Typisch een functionaliteit voor medewerkers.
        /// </summary>
        /// <param name="reserveringId">Het unieke ID van de reservering.</param>
        Task ApproveReserveringAsync(int reserveringId);

        /// <summary>
        /// Wijs een reservering af of markeer als geannuleerd.
        /// </summary>
        /// <param name="reserveringId">Het unieke ID van de reservering.</param>
        Task RejectReserveringAsync(int reserveringId);
    }
}