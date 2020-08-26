using EncoreTickets.SDK.Utilities.CommonModels;

namespace EncoreTickets.SDK.Pricing.Models
{
    public class Price : IPriceWithCurrency
    {
        public int? DecimalPlaces { get; set; }

        public int? Value { get; set; }

        public string Currency { get; set; }
    }
}
