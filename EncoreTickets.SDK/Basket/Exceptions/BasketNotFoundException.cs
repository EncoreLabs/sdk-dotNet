using System;
using EncoreTickets.SDK.Api.Results;

namespace EncoreTickets.SDK.Basket.Exceptions
{
    /// <summary>
    /// The exception if a requested basket was not found.
    /// </summary>
    public class BasketNotFoundException : Exception
    {
        public override string Message => SourceException.Message;

        public ApiException SourceException { get; }

        public string BasketId { get; set; }

        public BasketNotFoundException(ApiException sourceException, string basketId)
        {
            SourceException = sourceException;
            BasketId = basketId;
        }
    }
}