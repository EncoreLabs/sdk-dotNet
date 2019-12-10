using EncoreTickets.SDK.Utilities.CommonModels;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Price : IPriceWithCurrency
    {
        public int? Value { get; set; }

        public string Currency { get; set; }

        public int? DecimalPlaces { get; set; }

        public override string ToString()
        {
            return $"{Currency}{Value / 100}";
        }
    }
}