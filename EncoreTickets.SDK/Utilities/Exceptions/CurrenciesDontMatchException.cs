using System;

namespace EncoreTickets.SDK.Utilities.Exceptions
{
    public class CurrenciesDontMatchException : Exception
    {
        public CurrenciesDontMatchException()
        {
        }

        public CurrenciesDontMatchException(string message) : base(message)
        {
        }

        public CurrenciesDontMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
