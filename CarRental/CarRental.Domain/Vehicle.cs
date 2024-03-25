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
                (startDate >= r.StartDate.AddDays(-1) && startDate <= r.EndDate.AddDays(1) ||
                endDate >= r.StartDate.AddDays(-1) && endDate <= r.EndDate.AddDays(1) ||
                startDate <= r.StartDate && endDate >= r.EndDate));
        }

        public bool HasPendingRentals()
        {
            return Rentals.Exists(r =>
                !r.Canceled && DateTime.Today <= r.EndDate
            );
        }
    }
}