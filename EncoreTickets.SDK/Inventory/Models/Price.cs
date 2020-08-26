using EncoreTickets.SDK.Utilities.CommonModels;
using EncoreTickets.SDK.Utilities.CommonModels.Extensions;

namespace EncoreTickets.SDK.Inventory.Models
{
    public class Price : IPriceWithCurrency
    {
        /// <inheritdoc />
        public int? Value { get; set; }

        /// <inheritdoc />
        public string Currency { get; set; }

        /// <inheritdoc />
        public int? DecimalPlaces { get; set; }

        /// <inheritdoc />
        public override string ToString() => this.ToStringFormat();
    }
}