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
    public class AppointmentServiceTests
    {
        private readonly Mock<IRepository<Appointment>> _appointmentRepositoryMock;
        private readonly Mock<IRepository<Holiday>> _holidayRepositoryMock;
        private readonly Mock<IRepository<AppointmentSetting>> _settingsRepositoryMock;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IRepository<Appointment>>();
            _holidayRepositoryMock = new Mock<IRepository<Holiday>>();
            _settingsRepositoryMock = new Mock<IRepository<AppointmentSetting>>();
            _appointmentService = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _holidayRepositoryMock.Object,
                _settingsRepositoryMock.Object
            );
        }

        [Fact]
        public async Task BookAppointmentAsync_ShouldBookAppointment_WhenValidRequest()
        {
            // Arrange
            var request = new AppointmentSchema
            {
                Date = new DateTime(2024, 7, 20),
                CustomerName = "Andrie Aditya"
            };

            _settingsRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new AppointmentSetting { Id = 1, MaxAppointmentsPerDay = 5 });

            _holidayRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Holiday>());

            _appointmentRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            // Act
            var result = await _appointmentService.BookAppointmentAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.CustomerName, result.CustomerName);
            _appointmentRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public async Task BookAppointmentAsync_ShouldThrowException_WhenOnHoliday()
        {
            // Arrange
            var request = new AppointmentSchema
            {
                Date = new DateTime(2024, 7, 20),
                CustomerName = "Andrie Aditya"
            };

            _settingsRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new AppointmentSetting { Id = 1, MaxAppointmentsPerDay = 5 });

            _holidayRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Holiday>
                {
                new Holiday { Date = new DateTime(2024, 7, 20) }
                });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _appointmentService.BookAppointmentAsync(request));
        }
    }
}
