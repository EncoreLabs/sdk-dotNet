using EncoreTickets.SDK.Checkout.Models.RequestModels;

namespace EncoreTickets.SDK.Checkout.Models
{
    public class PaymentInfo
    {
        public string PaymentId { get; set; }

        public PaymentType? PaymentType { get; set; }
    }
}
