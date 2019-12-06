using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Basket.Models;

namespace EncoreTickets.SDK.Basket.Exceptions
{
    /// <summary>
    /// The exception if an API request used an invalid promo code.
    /// </summary>
    public class InvalidPromoCodeException : ContextApiException
    {
        public Coupon Coupon { get; set; }

        public InvalidPromoCodeException(ContextApiException sourceException, Coupon coupon)
            : base(sourceException)
        {
            Coupon = coupon;
        }
    }
}