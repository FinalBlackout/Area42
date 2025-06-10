using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Area42.Domain.Entities;
using Area42.Application.Interfaces;
using Area42.Test.Mocks;

namespace Area42.Test
{
    [TestClass]
    public class ReserveringServiceTests
    {
        private IReserveringService _reserveringService;

        [TestInitialize]
        public async Task Setup()
        {
            // We gebruiken hier de FakeReserveringService voor de tests.
            _reserveringService = new FakeReserveringService();

            // Maak een testreservering aan met een beginstatus.
            var reservering = new Reservering
            {
                Id = 1,
                Status = "In behandeling",
                // Voeg eventueel andere benodigde properties toe
            };
            await _reserveringService.CreateReserveringAsync(reservering);
        }

        [TestMethod]
        public async Task RejectReserveringAsync_Should_Update_StatusToRejected()
        {
            // Arrange: De reservering met Id 1 is al aanwezig en heeft de status "In behandeling".

            // Act: Roep RejectReserveringAsync aan om de reservering af te wijzen.
            await _reserveringService.RejectReserveringAsync(1);

            // Haal alle reserveringen op (of zoek specifiek de reservering met Id 1)
            var reserveringen = await _reserveringService.GetAllReserveringenAsync();
            var updatedReservering = reserveringen.FirstOrDefault(r => r.Id == 1);

            // Assert: Controleer dat de status is aangepast naar "Geweigerd".
            Assert.IsNotNull(updatedReservering, "Reservering mag niet null zijn.");
            Assert.AreEqual("Geweigerd", updatedReservering.Status, "De status moet 'Geweigerd' zijn.");
        }
    }
}