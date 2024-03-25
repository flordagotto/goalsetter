using System;

namespace CarRental.Services.Exceptions
{
    [Serializable]
    public class PendingRentalException : Exception
    {
        public PendingRentalException() { }

        public PendingRentalException(string message)
            : base(message) { }

        public PendingRentalException(string message, Exception inner)
            : base(message, inner) { }
    }
}
