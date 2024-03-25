using AutoMapper;
using CarRental.DataAcces;
using CarRental.Domain;
using CarRental.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Services.Vehicles
{
    public class VehicleService : IVehicleService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(AppDbContext dbContext, IMapper mapper, ILogger<VehicleService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<VehicleDto>> GetAll()
        {
            try
            {
                var vehicles = await _dbContext.Vehicles.ToListAsync();
                return _mapper.Map<List<VehicleDto>>(vehicles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicles");
                throw;
            }
        }

        public async Task<VehicleDto> GetById(long id)
        {
            try
            {
                var vehicle = await _dbContext.Vehicles.Where(v => v.Id == id).FirstOrDefaultAsync();
                return _mapper.Map<VehicleDto>(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vehicle");
                throw;
            }
        }

        public async Task<VehicleDto> AddNewVehicle(VehicleDto vehicleDto)
        {
            try
            {
                var vehicle = _mapper.Map<Vehicle>(vehicleDto);

                await _dbContext.Vehicles.AddAsync(vehicle);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<VehicleDto>(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding vehicle");
                throw;
            }
        }

        public async Task<VehicleDto> Delete(long id)
        {
            try
            {
                var vehicle = await _dbContext.Vehicles.FindAsync(id) ?? throw new EntityNotFoundException($"The Vehicle with id {id} was not found");
                await _dbContext.Entry(vehicle).Collection(v => v.Rentals).LoadAsync();

                if (vehicle.HasPendingRentals())
                {
                    throw new EntityWithPendingRentalsException($"The vehicle with id {id} has pending rentals");
                }

                _dbContext.Vehicles.Remove(vehicle);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<VehicleDto>(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vehicle");
                throw;
            }
        }


    }
}