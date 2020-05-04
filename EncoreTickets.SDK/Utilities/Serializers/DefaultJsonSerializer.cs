using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace EncoreTickets.SDK.Utilities.Serializers
{
    public class DefaultJsonSerializer : BaseJsonSerializer
    {
        public DefaultJsonSerializer()
            : base(CreateSettings(null))
        {
        }

        public DefaultJsonSerializer(params JsonConverter[] extraConverters)
            : base(CreateSettings(extraConverters))
        {
        }

        protected DefaultJsonSerializer(JsonSerializerSettings settings) : base(settings)
        {
        }

        protected static JsonSerializerSettings CreateSettings(IList<JsonConverter> extraConverters)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            AddConvertersToSettings(settings, extraConverters);
            return settings;
        }

        private static void AddConvertersToSettings(JsonSerializerSettings settings,
            IList<JsonConverter> extraConverters)
        {
            var enumConverter = new StringEnumConverter(new CamelCaseNamingStrategy());
            settings.Converters.Add(enumConverter);
            if (extraConverters == null)
            {
                return;
            }

            foreach (var extraConverter in extraConverters)
            {
                settings.Converters.Add(extraConverter);
            }
        }
    }
}
