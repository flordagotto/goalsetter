using System;

namespace CarRental.Services.Exceptions
{
    [Serializable]
    public class EntityWithPendingRentalsException : Exception
    {
        public EntityWithPendingRentalsException() { }

        public EntityWithPendingRentalsException(string message)
            : base(message) { }

        public EntityWithPendingRentalsException(string message, Exception inner)
            : base(message, inner) { }
    }
}
