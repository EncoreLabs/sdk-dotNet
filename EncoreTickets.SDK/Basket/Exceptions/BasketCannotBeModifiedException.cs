using EncoreTickets.SDK.Api.Results.Exceptions;

namespace EncoreTickets.SDK.Basket.Exceptions
{
    /// <summary>
    /// The exception if an API request tried to modify a basket that was not available for change.
    /// </summary>
    public class BasketCannotBeModifiedException : ApiException
    {
        public string BasketId { get; set; }

        public BasketCannotBeModifiedException(ApiException sourceException, string basketId)
            : base(sourceException)
        {
            BasketId = basketId;
        }
    }
}
