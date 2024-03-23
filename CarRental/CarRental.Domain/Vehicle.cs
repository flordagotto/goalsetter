using System;
using System.Collections.Generic;
using System.Linq;

namespace CarRental.Domain
{
    public class Vehicle
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public decimal PricePerDay { get; set; }
        public List<Rental> Rentals { get; set; }

        public bool IsAvailable(DateTime startDate, DateTime endDate)
        {
            return !Rentals.Any(r => 
                !r.Canceled &&
                startDate >= r.StartDate && startDate <= r.EndDate ||
                endDate >= r.StartDate && endDate <= r.EndDate);
        }
    }
}