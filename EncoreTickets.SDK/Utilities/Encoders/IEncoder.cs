namespace EncoreTickets.SDK.Utilities.Encoders
{
    internal interface IEncoder<in TSource, out TEncoded>
    {
        TEncoded Encode(TSource sourceData);
    }
}
