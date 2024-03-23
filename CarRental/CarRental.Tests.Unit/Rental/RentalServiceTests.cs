using AutoMapper;
using CarRental.Api.AutoMapper;
using CarRental.DataAcces;
using CarRental.Domain;
using CarRental.Services.Rentals;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarRental.Tests.Unit
{
    public class RentalServiceTests : IDisposable
    {
        private RentalService _sut;

        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _dbContext;
        private readonly Mock<ILogger<RentalService>> _loggerMock;

        public RentalServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: "RentalTestingDb")
               .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
            _dbContext.Database.EnsureCreated();

            var config = new MapperConfiguration(opts => opts.AddProfile(typeof(MapperProfile)));
            var mapper = config.CreateMapper();

            _loggerMock = new Mock<ILogger<RentalService>>();

            _sut = new RentalService(_dbContext, mapper, _loggerMock.Object);
        }

        [Fact]
        public async Task Add_WhenAddRental_ShouldSaveTheRental()
        {
            var clientId = 1;
            var vehicleId = 1;

            _dbContext.Vehicles.Add(new Vehicle 
            { 
                Description = "Ford Focus",
                PricePerDay = 10
            });
            _dbContext.SaveChanges();

            var rental = new RentalDto
            {
                ClientId = clientId,
                VehicleId = vehicleId,
                StartDate = new DateTime(2021, 4, 1),
                EndDate = new DateTime(2021, 4, 10)
            };

            await _sut.AddNewRental(rental);

            _dbContext.Rentals.Count().Should().Be(1);
            var rentalAdded = await _dbContext.Rentals.FirstOrDefaultAsync();
            rentalAdded.Id.Should().Be(1);
            rentalAdded.ClientId.Should().Be(clientId);
            rentalAdded.VehicleId.Should().Be(vehicleId);
            rentalAdded.StartDate.Should().Be(new DateTime(2021, 4, 1));
            rentalAdded.EndDate.Should().Be(new DateTime(2021, 4, 10));
            rentalAdded.Price.Should().Be(90);
            rentalAdded.Canceled.Should().BeFalse();
        }

        [Fact]
        public async Task CancelRental_WhenCancelARental_ShouldUpdateCanceledProp()
        {
            _dbContext.Rentals.Add(new Rental 
            {
                ClientId = 2,
                VehicleId = 5,
                StartDate = new DateTime(2021, 4, 1),
                EndDate = new DateTime(2021, 4, 10),
            });
            _dbContext.SaveChanges();

            await _sut.CancelRental(1);

            var rentalAdded = await _dbContext.Rentals.FirstOrDefaultAsync();
            rentalAdded.Id.Should().Be(1);
            rentalAdded.ClientId.Should().Be(2);
            rentalAdded.VehicleId.Should().Be(5);
            rentalAdded.StartDate.Should().Be(new DateTime(2021, 4, 1));
            rentalAdded.EndDate.Should().Be(new DateTime(2021, 4, 10));
            rentalAdded.Canceled.Should().BeTrue();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
