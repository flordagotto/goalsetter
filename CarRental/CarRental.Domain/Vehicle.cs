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
            bool StartDateIsWithinTheDatesOfTheRent(Rental rental)
            {
                return startDate >= rental.StartDate.AddDays(-1) && startDate <= rental.EndDate.AddDays(1);
            }

            bool EndDateIsWithinTheDatesOfTheRent(Rental rental)
            {
                return endDate >= rental.StartDate.AddDays(-1) && endDate <= rental.EndDate.AddDays(1);
            }

            bool DatesOfTheRentAreWithinSentDates(Rental rental)
            {
                return startDate <= rental.StartDate && endDate >= rental.EndDate;
            }

            return !Rentals.Exists(r =>
                !r.Canceled &&
                (StartDateIsWithinTheDatesOfTheRent(r) ||
                EndDateIsWithinTheDatesOfTheRent(r) ||
                DatesOfTheRentAreWithinSentDates(r)));
        }

        public bool HasPendingRentals()
        {
            return Rentals.Exists(r =>
                !r.Canceled && DateTime.Today <= r.EndDate
            );
        }
    }
}