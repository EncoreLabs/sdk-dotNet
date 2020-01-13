using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EncoreTickets.SDK.Utilities.Serializers
{
    public class DefaultJsonSerializer : BaseJsonSerializer
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
