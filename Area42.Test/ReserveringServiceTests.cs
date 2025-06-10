using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Area42.Test.Mocks;

namespace Area42.Test
{
    [TestClass]
    public class ReserveringServiceTests
    {
        private IReserveringService _service;

        [TestInitialize]
        public void Setup()
        {
            // Maak een nieuwe instantie van de fake service voor elke test
            _service = new FakeReserveringService();
        }

        [TestMethod]
        public async Task CreateReserveringAsync_Should_Add_NewReservering()
        {
            // Arrange
            var newReservering = new Reservering
            {
                Id = 1,
                Status = "In behandeling",
                Startdatum = DateTime.Now,
                Einddatum = DateTime.Now.AddDays(1)
            };

            // Act
            await _service.CreateReserveringAsync(newReservering);
            var allReserveringen = await _service.GetAllReserveringenAsync();

            // Assert
            Assert.AreEqual(1, allReserveringen.Count, "Er moet 1 reservering in de lijst staan.");
            Assert.AreEqual("In behandeling", allReserveringen.First().Status, "De beginstatus komt niet overeen.");
        }

        [TestMethod]
        public async Task UpdateReserveringAsync_Should_Update_ReservationDetails()
        {
            // Arrange - eerst aanmaken
            var reservering = new Reservering
            {
                Id = 2,
                Status = "In behandeling",
                Startdatum = new DateTime(2025, 1, 1),
                Einddatum = new DateTime(2025, 1, 2)
            };
            await _service.CreateReserveringAsync(reservering);

            // Act - bijwerken
            var updatedReservering = new Reservering
            {
                Id = 2,
                Status = "Geüpdatet",
                Startdatum = new DateTime(2025, 2, 1),
                Einddatum = new DateTime(2025, 1, 2) // Einddatum ongewijzigd
            };
            await _service.UpdateReserveringAsync(updatedReservering);
            var result = (await _service.GetAllReserveringenAsync()).FirstOrDefault(r => r.Id == 2);

            // Assert
            Assert.IsNotNull(result, "Reservering zou aanwezig moeten zijn.");
            Assert.AreEqual("Geüpdatet", result.Status, "De status moet geüpdatet zijn.");
            Assert.AreEqual(new DateTime(2025, 2, 1), result.Startdatum, "De startdatum moet geüpdatet zijn.");
        }

        [TestMethod]
        public async Task DeleteReserveringAsync_Should_Remove_Reservation()
        {
            // Arrange - reservering aanmaken
            var reservering = new Reservering { Id = 3, Status = "In behandeling" };
            await _service.CreateReserveringAsync(reservering);
            var allBefore = await _service.GetAllReserveringenAsync();
            Assert.AreEqual(1, allBefore.Count, "Er moet 1 reservering aanwezig zijn vóór verwijdering.");

            // Act - verwijderen
            await _service.DeleteReserveringAsync(reservering.Id);
            var allAfter = await _service.GetAllReserveringenAsync();

            // Assert
            Assert.AreEqual(0, allAfter.Count, "De reservering zou verwijderd moeten zijn.");
        }

        [TestMethod]
        public async Task GetReserveringenVoorUserAsync_Should_Return_AllReserveringen()
        {
            // Arrange - maak twee reserveringen aan
            var reservering1 = new Reservering { Id = 4, Status = "In behandeling" };
            var reservering2 = new Reservering { Id = 5, Status = "In behandeling" };
            await _service.CreateReserveringAsync(reservering1);
            await _service.CreateReserveringAsync(reservering2);

            // Act - omdat de Fake geen filtering toepast, krijgen we alle reserveringen
            var dummyUser = new ClaimsPrincipal();
            var result = await _service.GetReserveringenVoorUserAsync(dummyUser);

            // Assert
            Assert.AreEqual(2, result.Count, "Alle reserveringen moeten teruggegeven worden.");
        }
    }
}