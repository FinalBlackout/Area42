using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Area42.Test.Mocks;

namespace Area42.Test
{
    [TestClass]
    public class AccommodatieServiceTests
    {
        private IAccommodatieService _service;

        [TestInitialize]
        public void Setup()
        {
            // Elke test start met een frisse instantie van de fake service.
            _service = new FakeAccommodatieService();
        }

        [TestMethod]
        public async Task CreateAccommodatieAsync_Should_Add_NewAccommodatie()
        {
            // Arrange
            var nieuweAccommodatie = new Accommodatie
            {
                Id = 1,
                Naam = "Hotel de Zon",
                Type = "Hotel",
                Capaciteit = 100,
                Beschrijving = "Een zonnig hotel in het centrum.",
                PrijsPerNacht = 150.00m,
                Faciliteiten = "Zwembad, WiFi, Restaurant",
                Status = "Beschikbaar",
                ImagePath = "path/to/hoteldezon.jpg"
            };

            // Act
            await _service.AddAccommodatieAsync(nieuweAccommodatie);
            var alleAccommodaties = await _service.GetAllAccommodatiesAsync();

            // Assert
            Assert.AreEqual(1, alleAccommodaties.Count, "Er moet 1 accommodatie in de lijst staan.");
            var accommodatie = alleAccommodaties.First();
            Assert.AreEqual("Hotel de Zon", accommodatie.Naam, "De naam komt niet overeen.");
            Assert.AreEqual("Hotel", accommodatie.Type);
            Assert.AreEqual(100, accommodatie.Capaciteit);
            Assert.AreEqual("Beschikbaar", accommodatie.Status);
        }


        [TestMethod]
        public async Task DeleteAccommodatieAsync_Should_Remove_Accommodatie()
        {
            // Arrange: voeg een accommodatie toe
            var accommodatie = new Accommodatie
            {
                Id = 3,
                Naam = "Appartement De Leeuw",
                Type = "Appartement",
                Capaciteit = 4,
                Beschrijving = "Gezellig appartement.",
                PrijsPerNacht = 120.00m,
                Faciliteiten = "Keuken, WiFi",
                Status = "Beschikbaar",
                ImagePath = "path/to/appartement.jpg"
            };

            await _service.AddAccommodatieAsync(accommodatie);
            var voorVerwijderen = await _service.GetAllAccommodatiesAsync();
            Assert.AreEqual(1, voorVerwijderen.Count, "Er moet 1 accommodatie aanwezig zijn vóór verwijdering.");

            // Act: verwijder de accommodatie
            await _service.DeleteAccommodatieAsync(accommodatie.Id);
            var naVerwijderen = await _service.GetAllAccommodatiesAsync();

            // Assert
            Assert.AreEqual(0, naVerwijderen.Count, "De accommodatie moet verwijderd zijn.");
        }
    }
}