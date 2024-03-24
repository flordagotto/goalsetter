using System;

namespace CarRental.Services.Exceptions
{
    [Serializable]
    public class DateRangeNotValidException : Exception
    {
        public DateRangeNotValidException() { }

        public DateRangeNotValidException(string message)
            : base(message) { }

        public DateRangeNotValidException(string message, Exception inner)
            : base(message, inner) { }
    }
}
