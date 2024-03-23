using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Services.Vehicles
{
    public interface IVehicleService
    {
        Task<List<VehicleDto>> GetAll();

        Task<VehicleDto> GetById(long id);

        Task<VehicleDto> AddNewVehicle(VehicleDto vehicleDto);

        Task<VehicleDto> Delete(long id);

    }
}