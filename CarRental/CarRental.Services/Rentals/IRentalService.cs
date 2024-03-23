using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Services.Rentals
{
    public interface IRentalService
    {
        Task<List<RentalDto>> GetAll();
        Task<RentalDto> GetById(long id);
        Task<RentalDto> AddNewRental(RentalDto rentalDto);
        Task<RentalDto> Delete(long id);
        Task<RentalDto> CancelRental(long Id);
    }
}