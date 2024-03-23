using AutoMapper;
using CarRental.DataAcces;
using CarRental.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Services.Clients
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientService> _logger;

        public ClientService(AppDbContext dbContext, IMapper mapper, ILogger<ClientService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ClientDto>> GetAll()
        {
            try
            {
                var clients = await _dbContext.Clients.ToListAsync();
                return _mapper.Map<List<ClientDto>>(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting clients");
                throw;
            }
        }

        public async Task<ClientDto> GetById(long id)
        {
            try
            {
                var client = await _dbContext.Clients.Where(c => c.Id == id).Include(c => c.Rentals).FirstOrDefaultAsync();
                var clientDto = _mapper.Map<ClientDto>(client);
                return clientDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting client");
                throw;
            }
        }

        public async Task<ClientDto> AddNewClient(ClientDto clientDto)
        {
            try
            {
                var client = _mapper.Map<Client>(clientDto);

                await _dbContext.Clients.AddAsync(client);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<ClientDto>(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding client");
                throw;
            }
        }

        public async Task<ClientDto> Delete(long id)
        {
            try
            {
                var client = await _dbContext.Clients.FindAsync(id);
                if (client == null)
                {
                    throw new Exception("The Client with id was not found");
                }

                _dbContext.Clients.Remove(client);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<ClientDto>(client);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting client");
                throw;
            }
        }
    }
}