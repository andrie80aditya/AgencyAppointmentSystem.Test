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
    public class AppointmentControllerTests
    {
        private readonly Mock<IAppointmentService> _appointmentServiceMock;
        private readonly AppointmentController _appointmentController;

        public AppointmentControllerTests()
        {
            _appointmentServiceMock = new Mock<IAppointmentService>();
            _appointmentController = new AppointmentController(_appointmentServiceMock.Object);
        }

        [Fact]
        public async Task BookAppointment_ShouldReturnOkResult_WhenValidRequest()
        {
            // Arrange
            var request = new AppointmentSchema
            {
                Date = new DateTime(2024, 7, 20),
                CustomerName = "Andrie Aditya"
            };

            var appointment = new Appointment
            {
                Id = 1,
                Date = new DateTime(2024, 7, 20),
                CustomerName = "Andrie Aditya",
                TokenNumber = 1
            };

            _appointmentServiceMock.Setup(service => service.BookAppointmentAsync(request))
                .ReturnsAsync(appointment);

            // Act
            var result = await _appointmentController.BookAppointment(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Appointment>(okResult.Value);
            Assert.Equal(appointment.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetAppointmentsForDate_ShouldReturnOkResult_WhenAppointmentsExist()
        {
            // Arrange
            var date = new DateTime(2024, 7, 20);
            var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, Date = date, CustomerName = "Andrie Aditya", TokenNumber = 1 }
        };

            _appointmentServiceMock.Setup(service => service.GetAppointmentsForDateAsync(date))
                .ReturnsAsync(appointments);

            // Act
            var result = await _appointmentController.GetAppointmentsForDate(date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Appointment>>(okResult.Value);
            Assert.Single(returnValue);
        }
    }
}
