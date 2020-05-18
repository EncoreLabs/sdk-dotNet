namespace EncoreTickets.SDK.Utilities.Encoders
{
    internal interface IDecoder<in TEncoded, out TSource>
    {
        TSource Decode(TEncoded encodedData);
    }
}