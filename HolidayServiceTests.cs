using AgencyAppointmentSystem.Models;
using AgencyAppointmentSystem.Repositories;
using AgencyAppointmentSystem.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointmentSystem.Test
{
    public class HolidayServiceTests
    {
        private readonly Mock<IRepository<Holiday>> _holidayRepositoryMock;
        private readonly HolidayService _holidayService;

        public HolidayServiceTests()
        {
            _holidayRepositoryMock = new Mock<IRepository<Holiday>>();
            _holidayService = new HolidayService(_holidayRepositoryMock.Object);
        }

        [Fact]
        public async Task AddHolidayAsync_ShouldAddHoliday_WhenValidRequest()
        {
            // Arrange
            var request = new HolidaySchema
            {
                Date = new DateTime(2024, 7, 21)
            };

            _holidayRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Holiday>());

            // Act
            var result = await _holidayService.AddHolidayAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Date, result.Date);
            _holidayRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Holiday>()), Times.Once);
        }

        [Fact]
        public async Task AddHolidayAsync_ShouldThrowException_WhenDuplicateHoliday()
        {
            // Arrange
            var request = new HolidaySchema
            {
                Date = new DateTime(2024, 7, 21)
            };

            _holidayRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Holiday>
                {
                new Holiday { Date = new DateTime(2024, 7, 21) }
                });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _holidayService.AddHolidayAsync(request));
        }
    }
}
