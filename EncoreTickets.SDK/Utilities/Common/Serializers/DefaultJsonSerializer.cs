using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    class DefaultJsonSerializer : JsonSerializer
    {
        protected override Newtonsoft.Json.JsonSerializer Serializer => new Newtonsoft.Json.JsonSerializer
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };
    }
}
