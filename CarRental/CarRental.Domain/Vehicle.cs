using System;
using System.Collections.Generic;

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
            return !Rentals.Exists(r =>
                !r.Canceled &&
                (startDate.Date >= r.StartDate.Date && startDate.Date <= r.EndDate.Date ||
                endDate.Date >= r.StartDate.Date && endDate.Date <= r.EndDate.Date || 
                startDate.Date <= r.StartDate.Date && endDate.Date >= r.EndDate.Date));
        }
    }
}