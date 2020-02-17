using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Utilities.Serializers
{
    public interface ISerializerWithDateFormat : RestSharp.Serializers.ISerializer, RestSharp.Deserializers.IDeserializer
    {
        DataFormat SerializedDataFormat { get; }

        string DateFormat { get; set; }
    }
}
