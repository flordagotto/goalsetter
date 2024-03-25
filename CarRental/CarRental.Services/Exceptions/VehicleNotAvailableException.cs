using System;

namespace CarRental.Services.Exceptions
{
    [Serializable]
    public class VehicleNotAvailableException : Exception
    {
        public VehicleNotAvailableException() { }

        public VehicleNotAvailableException(string message)
            : base(message) { }

        public VehicleNotAvailableException(string message, Exception inner)
            : base(message, inner) { }
    }
}
