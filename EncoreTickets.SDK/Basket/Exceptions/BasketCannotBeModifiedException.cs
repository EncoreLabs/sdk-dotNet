using System;
using EncoreTickets.SDK.Api.Results;

namespace EncoreTickets.SDK.Basket.Exceptions
{
    /// <summary>
    /// The exception if an API request tried to modify a basket that was not available for change.
    /// </summary>
    public class BasketCannotBeModifiedException : Exception
    {
        public override string Message => SourceException.Message;

        public ApiException SourceException { get; }

        public string BasketId { get; set; }

        public BasketCannotBeModifiedException(ApiException sourceException, string basketId)
        {
            SourceException = sourceException;
            BasketId = basketId;
        }
    }
}
