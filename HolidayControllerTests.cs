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
    public class HolidayControllerTests
    {
        private readonly Mock<IHolidayService> _holidayServiceMock;
        private readonly HolidayController _holidayController;

        public HolidayControllerTests()
        {
            _holidayServiceMock = new Mock<IHolidayService>();
            _holidayController = new HolidayController(_holidayServiceMock.Object);
        }

        [Fact]
        public async Task AddHoliday_ShouldReturnOkResult_WhenValidRequest()
        {
            // Arrange
            var request = new HolidaySchema
            {
                Date = new DateTime(2024, 7, 21)
            };

            var holiday = new Holiday
            {
                Id = 1,
                Date = new DateTime(2024, 7, 21)
            };

            _holidayServiceMock.Setup(service => service.AddHolidayAsync(request))
                .ReturnsAsync(holiday);

            // Act
            var result = await _holidayController.Addholiday(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Holiday>(okResult.Value);
            Assert.Equal(holiday.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetHolidayList_ShouldReturnOkResult_WhenHolidaysExist()
        {
            // Arrange
            var holidays = new List<Holiday>
        {
            new Holiday { Id = 1, Date = new DateTime(2024, 7, 21) }
        };

            _holidayServiceMock.Setup(service => service.GetHolidaysAsync())
                .ReturnsAsync(holidays);

            // Act
            var result = await _holidayController.GetHolidayList();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Holiday>>(okResult.Value);
            Assert.Single(returnValue);
        }
    }
}
