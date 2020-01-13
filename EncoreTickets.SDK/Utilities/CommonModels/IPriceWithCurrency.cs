namespace EncoreTickets.SDK.Utilities.CommonModels
{
    public interface IPriceWithCurrency
    {
        int? Value { get; set; }

        int? DecimalPlaces { get; set; }

        string Currency { get; set; }
    }
}
