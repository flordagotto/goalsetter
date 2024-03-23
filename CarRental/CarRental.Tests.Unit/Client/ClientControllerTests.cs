using AutoMapper;
using CarRental.Api.AutoMapper;
using CarRental.Api.Controllers;
using CarRental.Api.Models.Client;
using CarRental.Services.Clients;
using CarRental.Services.Rentals;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarRental.Tests.Unit
{
    public class ClientControllerTests
    {
        private Mock<IClientService> _serviceMock;
        private ClientsController _sut;

        public ClientControllerTests()
        {
            _serviceMock = new Mock<IClientService>();

            var config = new MapperConfiguration(opts => opts.AddProfile(typeof(MapperProfile)));
            var mapper = config.CreateMapper();

            _sut = new ClientsController(_serviceMock.Object, mapper);
        }

        [Fact]
        public async Task Post_WhenCreateAclient_ShouldReturnTheCreatedClient()
        {
            // Arrange
            var clientId = 10;
            var newClientName = "Jose";
            var newClientLastName = "Perez";

            _serviceMock.Setup(s => s.AddNewClient(It.IsAny<ClientDto>()))
                .ReturnsAsync(new ClientDto { Id = clientId, Name = newClientName, LastName = newClientLastName });

            var newClientRequest = new ClientRequestModel
            {
                Name = newClientName,
                LastName = newClientLastName
            };

            // Act
            var result = await _sut.Post(newClientRequest);

            // Assert
            result.Should().BeOfType(typeof(CreatedAtActionResult));
            var value = result.As<ObjectResult>().Value.As<ClientResponseModel>();

            value.Id.Should().Be(clientId);
            value.Name.Should().Be(newClientName);
            value.Lastname.Should().Be(newClientLastName);

            _serviceMock.Verify(s => s.AddNewClient(
                It.Is<ClientDto>(c => c.Name == newClientName && c.LastName == newClientLastName)),
                Times.Once);
        }

        [Fact]
        public async Task GetById_WhenGetAClientById_ShouldReturnTheClientWithHisRentals()
        {
            // Arrange
            var clientId = 10;
            var newClientName = "Jose";
            var newClientLastName = "Perez";
            var rentalId = 8;

            _serviceMock.Setup(s => s.GetById(It.IsAny<long>()))
               .ReturnsAsync(new ClientDto
               {
                   Id = clientId,
                   Name = newClientName,
                   LastName = newClientLastName,
                   Rentals = new List<RentalDto>
                   {
                       new RentalDto
                       {
                           Id = rentalId,
                           ClientId = clientId,
                           StartDate = new DateTime(2021,04,01),
                           EndDate = new DateTime(2021,04,10),
                       }
                   }
               });

            // Act
            var result = await _sut.GetById(clientId);

            // Assert
            result.Should().BeOfType(typeof(OkObjectResult));
            var value = result.As<ObjectResult>().Value.As<ClientResponseModel>();
            value.Id.Should().Be(clientId);
            value.Name.Should().Be(newClientName);
            value.Lastname.Should().Be(newClientLastName);

            value.Rentals.Should().HaveCount(1);
            value.Rentals.First().Id.Should().Be(rentalId);
            value.Rentals.First().ClientId.Should().Be(clientId);
            value.Rentals.First().StartDate.Should().Be(new DateTime(2021, 04, 01));
            value.Rentals.First().EndDate.Should().Be(new DateTime(2021, 04, 10));

            _serviceMock.Verify(s => s.GetById(It.Is<long>(v => v == clientId)), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenDeletesClient_ShouldReturnNoContent()
        {
            // Arrange
            var clientId = 10;

            _serviceMock.Setup(s => s.Delete(It.IsAny<long>()))
               .ReturnsAsync(new ClientDto { Id = clientId });

            // Act
            var result = await _sut.Delete(clientId);

            result.Should().BeOfType(typeof(NoContentResult));

            _serviceMock.Verify(s => s.Delete(It.Is<long>(v => v == clientId)), Times.Once);
        }
    }
}
