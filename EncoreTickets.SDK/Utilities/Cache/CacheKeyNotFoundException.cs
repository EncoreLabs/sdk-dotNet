using System;

namespace EncoreTickets.SDK.Utilities.Cache
{
    public class CacheKeyNotFoundException : Exception
    {
        public CacheKeyNotFoundException() : base() { }

        public CacheKeyNotFoundException(string message) : base(message) { }

        public CacheKeyNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
