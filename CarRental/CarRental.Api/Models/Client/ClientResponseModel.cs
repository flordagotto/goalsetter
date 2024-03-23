using CarRental.Api.Models.Rental;
using System.Collections.Generic;

namespace CarRental.Api.Models.Client
{
    public class ClientResponseModel
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public List<RentalResponseModel> Rentals { get; set; }
    }
}