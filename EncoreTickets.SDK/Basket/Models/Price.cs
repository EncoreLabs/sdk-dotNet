using EncoreTickets.SDK.Utilities.CommonModels;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Price : IPriceWithCurrency
    {
        public int? Value { get; set; }

        public string Currency { get; set; }

        public int? DecimalPlaces { get; set; }
    }
}