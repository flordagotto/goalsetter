using AutoMapper;
using CarRental.Api.AutoMapper;
using CarRental.Api.Controllers;
using CarRental.Api.Models.Vehicle;
using CarRental.Services.Vehicles;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CarRental.Tests.Unit
{
    public class VehicleControllerTests
    {
        private Mock<IVehicleService> _serviceMock;
        private VehiclesController _sut;

        public VehicleControllerTests()
        {
            _serviceMock = new Mock<IVehicleService>();

            var config = new MapperConfiguration(opts => opts.AddProfile(typeof(MapperProfile)));
            var mapper = config.CreateMapper();

            _sut = new VehiclesController(_serviceMock.Object, mapper);
        }

        [Fact]
        public async Task Post_WhenCreateAVehicle_ShouldReturnTheCreatedVehicle()
        {
            // Arrange
            var newVehicleDescription = "VW Golf";
            var newVehiclePrice = 10;

            _serviceMock.Setup(s => s.AddNewVehicle(It.IsAny<VehicleDto>()))
                .ReturnsAsync(new VehicleDto { Id = 1, Description = newVehicleDescription, PricePerDay = newVehiclePrice });

            var newVehicleRequest = new VehicleRequestModel
            {
                Description = newVehicleDescription,
                PricePerDay = newVehiclePrice
            };

            // Act
            var result = await _sut.Post(newVehicleRequest);

            // Assert
            result.Should().BeOfType(typeof(CreatedAtActionResult));
            var value = result.As<ObjectResult>().Value.As<VehicleResponseModel>();

            value.Id.Should().Be(1);
            value.Description.Should().Be(newVehicleDescription);
            value.PricePerDay.Should().Be(newVehiclePrice);

            _serviceMock.Verify(s => s.AddNewVehicle(It.Is<VehicleDto>(c => c.Description == newVehicleDescription
            && c.PricePerDay == newVehiclePrice)), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenDeletesAVehicle_ShouldReturnNoContent()
        {
            // Arrange
            var VehicleId = 10;

            _serviceMock.Setup(s => s.Delete(It.IsAny<long>()))
               .ReturnsAsync(new VehicleDto { Id = VehicleId });

            // Act
            var result = await _sut.Delete(VehicleId);

            result.Should().BeOfType(typeof(NoContentResult));

            _serviceMock.Verify(s => s.Delete(It.Is<long>(v => v == VehicleId)), Times.Once);
        }

    }
}
