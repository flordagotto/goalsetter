using System;

namespace CarRental.Services.Exceptions
{
    [Serializable]
    public class VehicleWithPendingRentalsException : Exception
    {
        public VehicleWithPendingRentalsException() { }

        public VehicleWithPendingRentalsException(string message)
            : base(message) { }

        public VehicleWithPendingRentalsException(string message, Exception inner)
            : base(message, inner) { }
    }
}
