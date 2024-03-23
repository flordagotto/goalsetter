using CarRental.Services.Rentals;
using System.Collections.Generic;

namespace CarRental.Services.Clients
{
    public class ClientDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public List<RentalDto> Rentals { get; set; }
    }
}