using EncoreTickets.SDK.Payment.Models;

namespace EncoreTickets.SDK.Payment.Extensions
{
    public static class PaymentMethodExtension
    {
        public static int GetMonthFromPaymentCardExpiredDate(this PaymentMethod paymentMethod)
            => paymentMethod?.ExpiryDate?.Month ?? default;

        public static int GetYearFromPaymentCardExpiredDate(this PaymentMethod paymentMethod)
            => paymentMethod?.ExpiryDate?.Year ?? default;
    }
}
