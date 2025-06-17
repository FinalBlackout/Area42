using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Area42.Domain.Entities;
using Area42.Infrastructure.Services;
using Area42.Application.Interfaces;
using Area42.Domain.Entities;
using Area42.Infrastructure.Data;
using Area42.Infrastructure.Services;

namespace YourProject.Tests.Services
{
    [TestClass]
    public class ReserveringServiceTests
    {
        private Mock<IReserveringRepository> _mockRepository;
        private ReserveringService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepository = new Mock<IReserveringRepository>();
            _service = new ReserveringService(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetReserveringenAsync_ShouldReturnExpectedList()
        {
            // Arrange
            var reserveringen = new List<Reservering>
            {
                new Reservering { Id = 1, AccommodatieId = 1, UserId = 42, Status = "Goedgekeurd" },
                new Reservering { Id = 2, AccommodatieId = 2, UserId = 42, Status = "In Behandeling" }
            };

            _mockRepository.Setup(r => r.GetReserveringenAsync("42"))
                           .ReturnsAsync(reserveringen);

            // Act
            var result = await _service.GetReserveringenAsync("42");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Goedgekeurd", result[0].Status);
            _mockRepository.Verify(r => r.GetReserveringenAsync("42"), Times.Once);
        }

        [TestMethod]
        public async Task CreateReserveringAsync_ShouldCallRepositoryCorrectly()
        {
            var nieuweReservering = new Reservering
            {
                AccommodatieId = 3,
                UserId = 21,
                Startdatum = DateTime.Today,
                Einddatum = DateTime.Today.AddDays(2),
                Status = "In Behandeling"
            };

            await _service.CreateReserveringAsync(nieuweReservering);

            _mockRepository.Verify(r => r.AddAsync(It.Is<Reservering>(res =>
                res.AccommodatieId == 3 &&
                res.UserId == 21 &&
                res.Status == "In Behandeling")), Times.Once);
        }

        [TestMethod]
        public async Task DeleteReserveringAsync_ShouldCallRepositoryOnce()
        {
            await _service.DeleteReserveringAsync(99);

            _mockRepository.Verify(r => r.DeleteAsync(99), Times.Once);
        }

        [TestMethod]
        public async Task ApproveReserveringAsync_ShouldCallRepositoryOnce()
        {
            await _service.ApproveReserveringAsync(7);

            _mockRepository.Verify(r => r.ApproveAsync(7), Times.Once);
        }

        [TestMethod]
        public async Task RejectReserveringAsync_ShouldCallRepositoryOnce()
        {
            await _service.RejectReserveringAsync(13);

            _mockRepository.Verify(r => r.RejectAsync(13), Times.Once);
        }

        [TestMethod]
        public async Task UpdateReserveringAsync_ShouldCallRepositoryWithUpdatedValues()
        {
            var updated = new Reservering
            {
                Id = 5,
                AccommodatieId = 2,
                UserId = 1,
                Startdatum = DateTime.Today,
                Einddatum = DateTime.Today.AddDays(3),
                Status = "Gewijzigd"
            };

            await _service.UpdateReserveringAsync(updated);

            _mockRepository.Verify(r => r.UpdateAsync(It.Is<Reservering>(r =>
                r.Id == 5 &&
                r.Status == "Gewijzigd")), Times.Once);
        }

        [TestMethod]
        public async Task GetReserveringByIdAsync_ShouldReturnCorrectResult()
        {
            var reservering = new Reservering { Id = 10, AccommodatieId = 2, UserId = 3 };

            _mockRepository.Setup(r => r.GetReserveringByIdAsync(10))
                           .ReturnsAsync(reservering);

            var result = await _service.GetReserveringByIdAsync(10);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.AccommodatieId);
            _mockRepository.Verify(r => r.GetReserveringByIdAsync(10), Times.Once);
        }
    }
}
