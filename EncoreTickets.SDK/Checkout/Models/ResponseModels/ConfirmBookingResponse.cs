using EncoreTickets.SDK.Api.Results.Response;

namespace EncoreTickets.SDK.Checkout.Models.ResponseModels
{
    internal class ConfirmBookingResponse : BaseWrappedApiResponse<ConfirmBookingResponseContent, string>
    {
        /// <inheritdoc/>
        public override string Data => Response.Result;
    }

    internal class ConfirmBookingResponseContent
    {
        public string Result { get; set; }
    }
}
