using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace EncoreTickets.SDK.Utilities.Serializers
{
    public class DefaultJsonSerializer : BaseJsonSerializer
    {
        public DefaultJsonSerializer(NamingStrategy enumNamingStrategy = null)
            : base(CreateSettings(null, enumNamingStrategy))
        {
        }

        public DefaultJsonSerializer(IEnumerable<JsonConverter> extraConverters, NamingStrategy enumNamingStrategy = null)
            : base(CreateSettings(extraConverters, enumNamingStrategy))
        {
        }

        protected DefaultJsonSerializer(JsonSerializerSettings settings) : base(settings)
        {
        }

        protected static JsonSerializerSettings CreateSettings(
            IEnumerable<JsonConverter> extraConverters,
            NamingStrategy enumNamingStrategy)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            };
            AddConvertersToSettings(settings, extraConverters, enumNamingStrategy);
            return settings;
        }

        private static void AddConvertersToSettings(
            JsonSerializerSettings settings,
            IEnumerable<JsonConverter> extraConverters,
            NamingStrategy enumNamingStrategy)
        {
            var defaultConverters = GetDefaultConverters(enumNamingStrategy);
            var allConverters = extraConverters == null
                ? defaultConverters
                : defaultConverters.Concat(extraConverters);
            foreach (var converter in allConverters)
            {
                settings.Converters.Add(converter);
            }
        }

        private static IEnumerable<JsonConverter> GetDefaultConverters(NamingStrategy enumNamingStrategy)
        {
            return new List<JsonConverter>
            {
                new StringEnumConverter(enumNamingStrategy ?? new CamelCaseNamingStrategy())
            };
        }
    }
}
