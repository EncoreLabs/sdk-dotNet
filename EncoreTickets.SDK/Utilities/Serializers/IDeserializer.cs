namespace EncoreTickets.SDK.Utilities.Serializers
{
    public interface IDeserializer
    {
        T Deserialize<T>(string content);
    }
}