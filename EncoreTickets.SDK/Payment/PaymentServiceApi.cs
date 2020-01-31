using EncoreTickets.SDK.Api;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Api.Utilities.RequestExecutor;
using EncoreTickets.SDK.Payment.Models;
using EncoreTickets.SDK.Payment.Models.RequestModels;
using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Payment
{
    public class PaymentServiceApi : BaseApiWithAuthentication, IPaymentServiceApi
    {
        private const string PaymentApiHost = "payment-service.{0}tixuk.io/api/";

        public PaymentServiceApi(ApiContext context, bool automaticAuthentication = false)
            : base(context, PaymentApiHost, automaticAuthentication)
        {
        }

        /// <inheritdoc />
        public Order CreateOrder(CreateOrderRequest orderRequest)
        {
            TriggerAutomaticAuthentication();
            var parameters = new ExecuteApiRequestParameters
            {
                Endpoint = "v1/orders",
                Method = RequestMethod.Post,
                Body = orderRequest,
                ErrorWrappings = new[] { ErrorWrapping.MessageWithCode, ErrorWrapping.Context, }
            };
            var result = Executor.ExecuteApiWithWrappedResponse<Order>(parameters);
            return result.DataOrException;
        }
    }
}