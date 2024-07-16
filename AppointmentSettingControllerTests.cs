using AgencyAppointmentSystem.Controllers;
using AgencyAppointmentSystem.Models;
using AgencyAppointmentSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointmentSystem.Test
{
    public class AppointmentSettingControllerTests
    {
        private readonly Mock<IAppointmentSettingsService> _settingServiceMock;
        private readonly AppointmentSettingController _settingController;

        public AppointmentSettingControllerTests()
        {
            _settingServiceMock = new Mock<IAppointmentSettingsService>();
            _settingController = new AppointmentSettingController(_settingServiceMock.Object);
        }

        [Fact]
        public async Task AddAppointmentSetting_ShouldReturnOkResult_WhenValidRequest()
        {
            // Arrange
            var request = new AppointmentSettingSchema
            {
                MaxAppointmentsPerDay = 5
            };

            var setting = new AppointmentSetting
            {
                Id = 1,
                MaxAppointmentsPerDay = 5
            };

            _settingServiceMock.Setup(service => service.AddAppointmentSettingAsync(request))
                .ReturnsAsync(setting);

            // Act
            var result = await _settingController.AddAppointmentSetting(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<AppointmentSetting>(okResult.Value);
            Assert.Equal(setting.Id, returnValue.Id);
        }

        [Fact]
        public async Task AddAppointmentSetting_ShouldReturnBadRequest_WhenSettingAlreadyExists()
        {
            // Arrange
            var request = new AppointmentSettingSchema
            {
                MaxAppointmentsPerDay = 5
            };

            _settingServiceMock.Setup(service => service.AddAppointmentSettingAsync(request))
                .ThrowsAsync(new System.Exception("You can only set it once!"));

            // Act
            var result = await _settingController.AddAppointmentSetting(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("You can only set it once!", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAppointmentSettingList_ShouldReturnOkResult_WhenSettingsExist()
        {
            // Arrange
            var settings = new List<AppointmentSetting>
            {
                new AppointmentSetting { Id = 1, MaxAppointmentsPerDay = 5 }
            };

            _settingServiceMock.Setup(service => service.GetAppointmentSettingAsync())
                .ReturnsAsync(settings);

            // Act
            var result = await _settingController.GetAppointmentSettingList();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<AppointmentSetting>>(okResult.Value);
            Assert.Single(returnValue);
        }
    }
}
