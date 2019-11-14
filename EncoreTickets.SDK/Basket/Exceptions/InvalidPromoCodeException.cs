using System;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Basket.Models;

namespace EncoreTickets.SDK.Basket.Exceptions
{
    /// <summary>
    /// The exception if an API request used an invalid promo code.
    /// </summary>
    public class InvalidPromoCodeException : Exception
    {
        public override string Message => SourceException.Message;

        public ContextApiException SourceException { get; }

        public Coupon Coupon { get; set; }

        public InvalidPromoCodeException(ContextApiException sourceException, Coupon coupon)
        {
            SourceException = sourceException;
            Coupon = coupon;
        }
    }
}