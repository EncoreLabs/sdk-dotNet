using RestSharp.Deserializers;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    public interface IDeserializerWithDateFormat : IDeserializer
    {
        string DateFormat { set; }
    }
}
