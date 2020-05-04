namespace EncoreTickets.SDK.Payment.Models.RequestModels
{
    public class CreatePaymentRequest
    {
        public string OrderId { get; set; }

        public Amount Amount { get; set; }

        public Amount AmountOriginal { get; set; }
    }
}