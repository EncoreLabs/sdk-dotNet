using EncoreTickets.SDK.Utilities.CommonModels;

namespace EncoreTickets.SDK.Basket.Models
{
    public class Price : IPriceWithCurrency
    {
        public int? value { get; set; }

        public string currency { get; set; }

        public int? decimalPlaces { get; set; }
    }
}