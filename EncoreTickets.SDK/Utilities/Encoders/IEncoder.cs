namespace EncoreTickets.SDK.Utilities.Encoders
{
    internal interface IEncoder
    {
        string Encode(string text);

        string Decode(string encodedText);
    }
}
