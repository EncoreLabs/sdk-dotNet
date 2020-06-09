namespace EncoreTickets.SDK.Pricing.Models
{
    public class Partner
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string OfficeId { get; set; }

        public string CurrencyCode { get; set; }

        public string DefaultDisplayCurrencyCode { get; set; }

        public string Description { get; set; }

        public PartnerGroup PartnerGroup { get; set; }
    }
}
