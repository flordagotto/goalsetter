using AutoMapper;
using CarRental.Api.AutoMapper;
using CarRental.DataAcces;
using CarRental.Domain;
using CarRental.Services.Clients;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarRental.Tests.Unit
{
    public class ClientServiceTests : IDisposable
    {
        private ClientService _sut;

        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _dbContext;
        private readonly Mock<ILogger<ClientService>> _loggerMock;

        public ClientServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: "ClientTestingDb")
               .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
            _dbContext.Database.EnsureCreated();

            var config = new MapperConfiguration(opts => opts.AddProfile(typeof(MapperProfile)));
            var mapper = config.CreateMapper();

            _loggerMock = new Mock<ILogger<ClientService>>();

            _sut = new ClientService(_dbContext, mapper, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_WhenQueryAllClients_ShouldReturnAllClientsInDB()
        {
            _dbContext.Clients.Add(new Client { Name = "Andres", LastName = "Jimenez" });
            _dbContext.Clients.Add(new Client { Name = "Ivan", LastName = "Martinez" });
            _dbContext.Clients.Add(new Client { Name = "Juan", LastName = "Ramirez" });
            await _dbContext.SaveChangesAsync();

            var clients = await _sut.GetAll();

            clients.Should().BeOfType(typeof(List<ClientDto>));
            clients.Should().HaveCount(3);

            clients.First().Id.Should().Be(1);
            clients.First().Name.Should().Be("Andres");
            clients.First().LastName.Should().Be("Jimenez");
        }

        [Fact]
        public async Task GetById_WhenQueryClientById_ShouldReturnTheClient()
        {
            _dbContext.Clients.Add(new Client { Name = "Andres", LastName = "Jimenez" });
            _dbContext.Clients.Add(new Client { Name = "Ivan", LastName = "Martinez" });
            _dbContext.Clients.Add(new Client { Name = "Juan", LastName = "Ramirez" });
            await _dbContext.SaveChangesAsync();

            var clients = await _sut.GetById(2);

            clients.Should().BeOfType(typeof(ClientDto));

            clients.Name.Should().Be("Ivan");
            clients.LastName.Should().Be("Martinez");
        }

        [Fact]
        public async Task Add_WhenAddClient_ShouldSaveClient()
        {
            var client = new ClientDto
            {
                Name = "Ivan",
                LastName = "Martinez"
            };

            var clientDto = await _sut.AddNewClient(client);

            clientDto.Should().NotBeNull();
            clientDto.Id.Should().Be(1);
            clientDto.Name.Should().Be("Ivan");
            clientDto.LastName.Should().Be("Martinez");
        }

        [Fact]
        public async Task Delete_WhenDeleteAClient_ShouldNotExistsInDB()
        {
            _dbContext.Clients.Add(new Client { Name = "Andres", LastName = "Jimenez" });
            _dbContext.Clients.Add(new Client { Name = "Ivan", LastName = "Martinez" });
            _dbContext.Clients.Add(new Client { Name = "Juan", LastName = "Ramirez" });
            await _dbContext.SaveChangesAsync();

            var deletedClientDto = await _sut.Delete(2);

            deletedClientDto.Name.Should().Be("Ivan");
            deletedClientDto.LastName.Should().Be("Martinez");

            var clients = await _dbContext.Clients.ToListAsync();
            clients.Should().HaveCount(2);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
