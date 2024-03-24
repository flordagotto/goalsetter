using System;

namespace CarRental.Services.Exceptions
{
    [Serializable]
    public class ClientNotFoundException : Exception
    {
        public ClientNotFoundException() { }

        public ClientNotFoundException(string message)
            : base(message) { }

        public ClientNotFoundException(string message, Exception inner)
            : base(message, inner) { }
    }
}
