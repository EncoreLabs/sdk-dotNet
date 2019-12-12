using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    internal class DefaultJsonSerializer : JsonSerializer
    {
        protected override JsonSerializerSettings Settings { get; }  = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };
    }
}
