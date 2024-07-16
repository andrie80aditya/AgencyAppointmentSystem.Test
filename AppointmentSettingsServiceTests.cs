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
    public class AppointmentSettingsServiceTests
    {
        private readonly Mock<IRepository<AppointmentSetting>> _settingsRepositoryMock;
        private readonly AppointmentSettingsService _settingsService;

        public AppointmentSettingsServiceTests()
        {
            _settingsRepositoryMock = new Mock<IRepository<AppointmentSetting>>();
            _settingsService = new AppointmentSettingsService(_settingsRepositoryMock.Object);
        }

        [Fact]
        public async Task AddAppointmentSettingAsync_ShouldAddSetting_WhenValidRequest()
        {
            // Arrange
            var request = new AppointmentSettingSchema
            {
                MaxAppointmentsPerDay = 5
            };

            _settingsRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<AppointmentSetting>());

            // Act
            var result = await _settingsService.AddAppointmentSettingAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.MaxAppointmentsPerDay, result.MaxAppointmentsPerDay);
            _settingsRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<AppointmentSetting>()), Times.Once);
        }

        [Fact]
        public async Task AddAppointmentSettingAsync_ShouldThrowException_WhenSettingExists()
        {
            // Arrange
            var request = new AppointmentSettingSchema
            {
                MaxAppointmentsPerDay = 5
            };

            _settingsRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<AppointmentSetting>
                {
                new AppointmentSetting { Id = 1, MaxAppointmentsPerDay = 5 }
                });

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _settingsService.AddAppointmentSettingAsync(request));
        }
    }
}
