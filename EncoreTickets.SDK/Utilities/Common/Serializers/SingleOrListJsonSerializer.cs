using Newtonsoft.Json;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    public class SingleOrListJsonSerializer<T> : DefaultJsonSerializer
    {
        public SingleOrListJsonSerializer() : base(CreateSettings())
        {
        }

        private new static JsonSerializerSettings CreateSettings()
        {
            var settings = DefaultJsonSerializer.CreateSettings();
            settings.Converters.Add(new SingleOrListConverter<T>());
            return settings;
        }
    }
}
