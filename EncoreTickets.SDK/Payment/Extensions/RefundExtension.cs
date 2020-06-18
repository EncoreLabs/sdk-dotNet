using EncoreTickets.SDK.Payment.Constants;
using EncoreTickets.SDK.Payment.Models;
using EncoreTickets.SDK.Utilities.CommonModels.Extensions;

namespace EncoreTickets.SDK.Payment.Extensions
{
    public static class RefundExtension
    {
        public static bool IsSuccessfulRefund(this Refund refund)
        {
            var statuses = new[]
            {
                RefundApiConstants.RefundStatusReceived,
                RefundApiConstants.RefundStatusPending,
                RefundApiConstants.RefundStatusSuccess,
            };
            return refund.HasOneOfStatuses(statuses);
        }
    }
}
