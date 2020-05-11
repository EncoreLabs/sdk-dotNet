using System;
using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Constants;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Checkout.Models;
using EncoreTickets.SDK.Checkout.Models.RequestModels;
using EncoreTickets.SDK.Checkout.Models.ResponseModels;
using EncoreTickets.SDK.Utilities.Enums;
using EncoreTickets.SDK.Utilities.Serializers;
using Newtonsoft.Json.Serialization;

namespace EncoreTickets.SDK.Checkout
{
    /// <inheritdoc cref="BaseApi" />
    /// <inheritdoc cref="ICheckoutServiceApi" />
    /// <summary>
    /// The wrapper class for the checkout service API.
    /// </summary>
    public class CheckoutServiceApi : BaseApi, ICheckoutServiceApi
    {
        private const string CheckoutApiHost = "checkout-service.{0}tixuk.io/api/";

        /// <inheritdoc/>
        public override int? ApiVersion => 1;

        /// <summary>
        /// Default constructor for the checkout service
        /// </summary>
        /// <param name="context"></param>
        public CheckoutServiceApi(ApiContext context) : base(context, CheckoutApiHost)
        {
        }

        /// <inheritdoc />
        public PaymentInfo Checkout(BookingParameters bookingParameters)
        {
            if (bookingParameters == null)
            {
                throw new ArgumentException("booking parameters must be set");
            }

            var requestParameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/checkout",
                Method = RequestMethod.Post,
                Body = bookingParameters,
                Serializer = new DefaultJsonSerializer(new DefaultNamingStrategy())
            };
            var result = Executor.ExecuteApiWithWrappedResponse<PaymentInfo>(requestParameters);
            return result.DataOrException;
        }

        /// <inheritdoc />
        public bool ConfirmBooking(string bookingReference, ConfirmBookingParameters bookingParameters)
        {
            if (string.IsNullOrWhiteSpace(bookingReference))
            {
                throw new ArgumentException("booking reference must not be empty");
            }

            if (bookingParameters == null)
            {
                throw new ArgumentException("parameters must be set");
            }

            var requestParameters = new ExecuteApiRequestParameters
            {
                Endpoint = $"v{ApiVersion}/bookings/{bookingReference}/confirm",
                Method = RequestMethod.Post,
                Body = bookingParameters
            };
            var result = Executor.ExecuteApiWithWrappedResponse<string, ConfirmBookingResponse, ConfirmBookingResponseContent>(
                    requestParameters);
            return result.DataOrException.Equals(ActionResultStatuses.Success, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
