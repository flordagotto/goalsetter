using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Domain
{
    public class Client
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public List<Rental> Rentals { get; set; }

        public bool HasPendingRentals()
        {
            return Rentals.Exists(r =>
                !r.Canceled && DateTime.Today <= r.EndDate
            );
        }
    }
}