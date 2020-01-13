using EncoreTickets.SDK.Utilities.Serializers.Converters;
using Newtonsoft.Json;

namespace EncoreTickets.SDK.Utilities.Serializers
{
    public class SingleOrListJsonSerializer<T> : DefaultJsonSerializer
    {
        public SingleOrListJsonSerializer() : base(CreateSettings())
        {
        }

        private new static JsonSerializerSettings CreateSettings()
        {
            var settings = DefaultJsonSerializer.CreateSettings();
            var converter = new SingleOrListConverter<T>();
            settings.Converters.Add(converter);
            return settings;
        }
    }
}
