using AutoMapper;
using CarRental.Api.AutoMapper;
using CarRental.DataAcces;
using CarRental.Domain;
using CarRental.Services.Vehicles;
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
    public class VehicleServiceTests : IDisposable
    {
        private VehicleService _sut;

        private readonly DbContextOptions<AppDbContext> _dbContextOptions;
        private readonly AppDbContext _dbContext;
        private readonly Mock<ILogger<VehicleService>> _loggerMock;

        public VehicleServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: "VehicleTestingDb")
               .Options;

            _dbContext = new AppDbContext(_dbContextOptions);
            _dbContext.Database.EnsureCreated();

            var config = new MapperConfiguration(opts => opts.AddProfile(typeof(MapperProfile)));
            var mapper = config.CreateMapper();

            _loggerMock = new Mock<ILogger<VehicleService>>();

            _sut = new VehicleService(_dbContext, mapper, _loggerMock.Object);
        }

        [Fact]
        public async Task Add_WhenAddVehicle_ShouldSaveTheVehicle()
        {
            var vehicle = new VehicleDto
            {
                Description = "VW Golf",
                PricePerDay = 10
            };

            var vehicleDto = await _sut.AddNewVehicle(vehicle);

            vehicleDto.Should().NotBeNull();
            vehicleDto.Id.Should().Be(1);
            vehicleDto.Description.Should().Be("VW Golf");
            vehicleDto.PricePerDay.Should().Be(10);

            _dbContext.Vehicles.Count().Should().Be(1);
            var vehicleAdded = await _dbContext.Vehicles.FirstOrDefaultAsync();
            vehicleAdded.Id.Should().Be(1);
            vehicleAdded.Description.Should().Be("VW Golf");
            vehicleAdded.PricePerDay.Should().Be(10);
        }

        [Fact]
        public async Task Delete_WhenDeleteAVehicle_ShouldNotExistsInDB()
        {
            _dbContext.Vehicles.Add(new Vehicle { Description = "VW Golf", PricePerDay = 10 });
            _dbContext.Vehicles.Add(new Vehicle { Description = "Ford Focus", PricePerDay = 12 });
            await _dbContext.SaveChangesAsync();

            var deletedVehicleDto = await _sut.Delete(2);

            deletedVehicleDto.Description.Should().Be("Ford Focus");
            deletedVehicleDto.PricePerDay.Should().Be(12);

            var Vehicles = await _dbContext.Vehicles.ToListAsync();
            Vehicles.Should().HaveCount(1);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
