using System;

namespace CarRental.Services.Exceptions
{
    [Serializable]
    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException() { }

        public VehicleNotFoundException(string message)
            : base(message) { }

        public VehicleNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}
