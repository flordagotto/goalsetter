using AutoMapper;
using CarRental.Api.AutoMapper;
using CarRental.Api.Controllers;
using CarRental.Api.Models.Rental;
using CarRental.Services.Rentals;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CarRental.Tests.Unit
{
    public class RentalsControllerTests
    {
        private Mock<IRentalService> _serviceMock;
        private RentalsController _sut;

        public RentalsControllerTests()
        {
            _serviceMock = new Mock<IRentalService>();

            var config = new MapperConfiguration(opts => opts.AddProfile(typeof(MapperProfile)));
            var mapper = config.CreateMapper();

            _sut = new RentalsController(_serviceMock.Object, mapper);
        }

        [Fact]
        public async Task Post_WhenCreateARental_ShouldReturnTheRentalCreated()
        {
            // Arrange
            _serviceMock.Setup(s => s.AddNewRental(It.IsAny<RentalDto>()))
                .ReturnsAsync(new RentalDto
                {
                    Id = 1,
                    ClientId = 1,
                    StartDate = new DateTime(2021, 04, 1),
                    EndDate = new DateTime(2021, 04, 10)
                });

            var newRentalRequest = new RentalRequestModel
            {
                ClientId = 1,
                StartDate = new DateTime(2021, 04, 1),
                EndDate = new DateTime(2021, 04, 10)
            };

            // Act
            var result = await _sut.Post(newRentalRequest);

            // Assert
            result.Should().BeOfType(typeof(CreatedAtActionResult));
            var value = result.As<ObjectResult>().Value.As<RentalResponseModel>();

            value.Id.Should().Be(1);
            value.ClientId.Should().Be(1);
            value.StartDate.Should().Be(new DateTime(2021, 04, 1));
            value.EndDate.Should().Be(new DateTime(2021, 04, 10));

            _serviceMock.Verify(s => s.AddNewRental(It.Is<RentalDto>(r => r.ClientId == 1 &&
            r.StartDate == new DateTime(2021, 04, 1) &&
            r.EndDate == new DateTime(2021, 04, 10)
            )), Times.Once);
        }

        [Fact]
        public async Task Cancel_WhenCancelARental_ShouldReturnTheRentalCanceled()
        {
            var rentalId = 1;
            _serviceMock.Setup(s => s.CancelRental(It.IsAny<long>()))
               .ReturnsAsync(new RentalDto
               {
                   Id = rentalId,
                   ClientId = 1,
                   VehicleId = 1,
                   StartDate = new DateTime(2021, 04, 01),
                   EndDate = new DateTime(2021, 04, 10),
                   Canceled = true
               });

            var result = await _sut.Cancel(rentalId);

            result.Should().BeOfType(typeof(OkObjectResult));
            var rentalCanceled = result.As<ObjectResult>().Value.As<RentalResponseModel>();

            rentalCanceled.Id.Should().Be(rentalId);
            rentalCanceled.Canceled.Should().BeTrue();

            _serviceMock.Verify(s => s.CancelRental(It.Is<long>(v => v == rentalId)), Times.Once);
        }
    }
}
