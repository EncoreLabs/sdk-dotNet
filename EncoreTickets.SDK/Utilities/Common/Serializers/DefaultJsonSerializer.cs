using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    public class DefaultJsonSerializer : JsonSerializer
    {
        public DefaultJsonSerializer() : base(CreateSettings())
        {
        }

        protected DefaultJsonSerializer(JsonSerializerSettings settings) : base(settings)
        {
        }

        protected static JsonSerializerSettings CreateSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }
    }
}
