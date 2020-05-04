namespace EncoreTickets.SDK.Payment.Models
{
    public class RiskData
    {
        public string DeliveryMethod { get; set; }

        public int? OfficeId { get; set; }

        public int? DaysToEvent { get; set; }
    }
}