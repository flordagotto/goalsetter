using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Domain
{
    public class Rental
    {
        public long Id { get; set; }

        public long ClientId { get; set; }

        public Client Client { get; set; }

        public long VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public bool Canceled { get; set; }

        public void SetPrice(decimal pricePerDay)
        {
            Price = (EndDate - StartDate).Days * pricePerDay;
        }
    }
}