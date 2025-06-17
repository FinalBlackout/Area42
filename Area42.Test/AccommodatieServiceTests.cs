using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Area42.Infrastructure.Services;
using Area42.Infrastructure.Data;

namespace Area42.Tests.Unit
{
    [TestClass]
    public class AccommodatieServiceTests
    {
        private Mock<IAccommodatieRepository> _mockRepository;
        private IAccommodatieService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IAccommodatieRepository>();
            _service = new AccommodatieService(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetAllAccommodatiesAsync_ShouldReturnAccommodations()
        {
            //Arrange
            var accommodaties = new List<Accommodatie>
            {
                new Accommodatie { Id = 1, Naam = "Hotel Suite", Type = "Suite", Capaciteit = 2, PrijsPerNacht = 200.0m, Status = "Beschikbaar", ImagePath = null },
                new Accommodatie { Id = 2, Naam = "Standaard Kamer", Type = "Standard", Capaciteit = 1, PrijsPerNacht = 100.0m, Status = "Beschikbaar", ImagePath = null }
            };

            _mockRepository.Setup(repo => repo.GetAllAccommodatiesAsync())
                           .ReturnsAsync(accommodaties);

            //Act
            var result = await _service.GetAllAccommodatiesAsync();

            //Assert
            Assert.IsNotNull(result, "Result mag niet null zijn.");
            Assert.AreEqual(2, result.Count, "Er moeten 2 accommodaties worden teruggegeven.");
            Assert.AreEqual("Hotel Suite", result[0].Naam);
            Assert.AreEqual(200.0m, result[0].PrijsPerNacht);

            _mockRepository.Verify(repo => repo.GetAllAccommodatiesAsync(), Times.Once);
        }
        [TestMethod]
        public async Task AddAccommodatieAsync_ShouldCallRepositoryWithCorrectObject()
        {
            // Arrange
            var accommodatie = new Accommodatie
            {
                Id = 3,
                Naam = "Economy Room",
                Type = "Budget",
                Capaciteit = 1,
                PrijsPerNacht = 80.0m,
                Status = "Beschikbaar",
                ImagePath = null
            };

            // Act
            await _service.AddAccommodatieAsync(accommodatie);

            // Assert
            _mockRepository.Verify(repo => repo.AddAccommodatieAsync(It.Is<Accommodatie>(a =>
                a.Naam == "Economy Room" &&
                a.Type == "Budget" &&
                a.Capaciteit == 1 &&
                a.PrijsPerNacht == 80.0m &&
                a.Status == "Beschikbaar")), Times.Once);
        }
        [TestMethod]
        public async Task DeleteAccommodatieAsync_ShouldCallRepositoryWithCorrectId()
        {
            // Arrange
            int idToDelete = 42;

            // Act
            await _service.DeleteAccommodatieAsync(idToDelete);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAccommodatieAsync(idToDelete), Times.Once);
        }
    }
}