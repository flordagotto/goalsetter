using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Services.Clients
{
    public interface IClientService
    {
        Task<List<ClientDto>> GetAll();

        Task<ClientDto> GetById(long id);

        Task<ClientDto> AddNewClient(ClientDto clientDto);

        Task<ClientDto> Delete(long id);

    }
}