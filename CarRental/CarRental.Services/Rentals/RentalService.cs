using AutoMapper;
using CarRental.DataAcces;
using CarRental.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Services.Rentals
{
    public class RentalService : IRentalService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RentalService> _logger;

        public RentalService(AppDbContext dbContext, IMapper mapper, ILogger<RentalService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<RentalDto>> GetAll()
        {
            try
            {
                var rentals = await _dbContext.Rentals.ToListAsync();
                return _mapper.Map<List<RentalDto>>(rentals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting rentals");
                throw;
            }
        }

        public async Task<RentalDto> GetById(long id)
        {
            try
            {
                var rental = await _dbContext.Rentals.Where(c => c.Id == id).FirstOrDefaultAsync();
                return _mapper.Map<RentalDto>(rental);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting rental");
                throw;
            }
        }

        public async Task<RentalDto> AddNewRental(RentalDto rentalDto)
        {
            try
            {
                var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.Id == rentalDto.VehicleId);

                if (vehicle == null)
                {
                    throw new Exception("The Vehicle selected does not exist");
                }

                await _dbContext.Entry(vehicle).Collection(v => v.Rentals).LoadAsync();
                if (!vehicle.IsAvailable(rentalDto.StartDate, rentalDto.EndDate))
                {
                    throw new Exception("The Vehicle is not available for the selected date range");
                }

                var rental = _mapper.Map<Rental>(rentalDto);
                rental.SetPrice(vehicle.PricePerDay);

                await _dbContext.Rentals.AddAsync(rental);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<RentalDto>(rental);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding rental");
                throw;
            }
        }

        public async Task<RentalDto> Delete(long id)
        {
            try
            {
                var rental = await _dbContext.Rentals.FindAsync(id);
                if (rental == null)
                {
                    throw new Exception("The rental with id was not found");
                }

                _dbContext.Rentals.Remove(rental);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<RentalDto>(rental);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting rental");
                throw;
            }
        }

        public async Task<RentalDto> CancelRental(long rentalId)
        {
            try
            {
                var rental = await _dbContext.Rentals.FirstOrDefaultAsync(r => r.Id == rentalId);
                if (rental == null)
                {
                    throw new Exception("The Rental with id was not found");
                }

                rental.Canceled = true; 
                _dbContext.SaveChanges();

                return _mapper.Map<RentalDto>(rental);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error canceling rental");
                throw;
            }
        }
    }
}