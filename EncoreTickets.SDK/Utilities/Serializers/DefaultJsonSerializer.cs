using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            var enumConverter = new StringEnumConverter(new CamelCaseNamingStrategy());
            settings.Converters.Add(enumConverter);
            return settings;
        }
    }
}
