using System;

namespace CarRental.Api.Models.Rental
{
    public class RentalResponseModel
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public long VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public bool Canceled { get; set; }
    }
}