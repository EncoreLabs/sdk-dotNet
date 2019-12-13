using RestSharp.Serializers;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    public interface ISerializerWithDateFormat : ISerializer
    {
        string DateFormat { set; }
    }
}
