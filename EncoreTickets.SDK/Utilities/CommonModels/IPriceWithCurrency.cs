namespace EncoreTickets.SDK.Utilities.CommonModels
{
    public interface IPriceWithCurrency
    {
        int? value { get; set; }

        int? decimalPlaces { get; set; }

        string currency { get; set; }
    }
}
