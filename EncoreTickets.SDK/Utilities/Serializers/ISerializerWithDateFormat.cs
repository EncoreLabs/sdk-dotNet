using EncoreTickets.SDK.Utilities.Enums;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace EncoreTickets.SDK.Utilities.Serializers
{
    public interface ISerializerWithDateFormat : ISerializer, IDeserializer
    {
        DataFormat SerializedDataFormat { get; }

        string DateFormat { get; set; }

        T Deserialize<T>(string content);
    }
}
