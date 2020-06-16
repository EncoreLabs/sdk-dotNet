using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Utilities.Serializers;
using EncoreTickets.SDK.Utilities.Serializers.Converters;

namespace EncoreTickets.SDK.Payment.Serializers
{
    internal class JsonResponseToTerritorialUnitsDeserializer : DefaultJsonSerializer
    {
        public JsonResponseToTerritorialUnitsDeserializer()
            : base(new[] { new SingleOrListToSingleConverter<Request>() })
        {
        }
    }
}