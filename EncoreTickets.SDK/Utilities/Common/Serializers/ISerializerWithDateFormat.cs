using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    public interface ISerializerWithDateFormat : ISerializer, IDeserializer
    {
        string DateFormat { get; set; }
    }
}
