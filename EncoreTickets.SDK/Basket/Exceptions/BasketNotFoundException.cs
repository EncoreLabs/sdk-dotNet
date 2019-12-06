using EncoreTickets.SDK.Api.Results.Exceptions;

namespace EncoreTickets.SDK.Basket.Exceptions
{
    /// <summary>
    /// The exception if a requested basket was not found.
    /// </summary>
    public class BasketNotFoundException : ApiException
    {
        public string BasketId { get; set; }

        public BasketNotFoundException(ApiException sourceException, string basketId) : base(sourceException)
        {
            BasketId = basketId;
        }
    }
}