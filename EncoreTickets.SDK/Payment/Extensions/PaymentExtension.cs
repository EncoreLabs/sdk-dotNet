using EncoreTickets.SDK.Payment.Constants;
using EncoreTickets.SDK.Utilities.CommonModels.Extensions;

namespace EncoreTickets.SDK.Payment.Extensions
{
    public static class PaymentExtension
    {
        public static bool IsSuccessfulPayment(this Models.Payment payment)
        {
            var statuses = new[]
            {
                PaymentApiConstants.PaymentStatusAuthorised,
                PaymentApiConstants.PaymentStatusCaptured,
                PaymentApiConstants.PaymentStatusCompensated,
                PaymentApiConstants.PaymentStatusPartiallyCompensated,
                PaymentApiConstants.PaymentStatusRefunded,
                PaymentApiConstants.PaymentStatusPartiallyRefunded,
            };
            return payment.HasOneOfStatuses(statuses);
        }

        public static bool IsAuthorizedPayment(this Models.Payment payment)
            => IsAuthorizedPaymentStatus(payment?.Status);

        public static bool IsAuthorizedPaymentStatus(string paymentStatus)
            => EntityWithStatusExtension.CompareStatuses(paymentStatus, PaymentApiConstants.PaymentStatusAuthorised);

        public static bool IsNewPayment(this Models.Payment payment)
            => IsNewPaymentStatus(payment?.Status);

        public static bool IsNewPaymentStatus(string paymentStatus)
            => EntityWithStatusExtension.CompareStatuses(paymentStatus, PaymentApiConstants.PaymentStatusNew);
    }
}