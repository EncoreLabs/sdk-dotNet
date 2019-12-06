using EncoreTickets.SDK.Utilities.CommonModels;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Price : IPriceWithCurrency
    {
        public int? value { get; set; }

        public string currency { get; set; }

        public int? decimalPlaces { get; set; }

        public override string ToString()
        {
            return $"{currency}{value / 100}";
        }
    }
}