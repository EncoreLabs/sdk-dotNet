using EncoreTickets.SDK.Utilities.CommonModels;
using EncoreTickets.SDK.Utilities.CommonModels.Extensions;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Price : IPriceWithCurrency
    {
        public int? Value { get; set; }

        public string Currency { get; set; }

        public int? DecimalPlaces { get; set; }

        public override string ToString() => this.ToStringFormat();
    }
}