using System;

namespace CarRental.Services.Exceptions
{
    [Serializable]
    public class RentalNotFoundException : Exception
    {
        public RentalNotFoundException() { }

        public RentalNotFoundException(string message)
            : base(message) { }

        public RentalNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}
