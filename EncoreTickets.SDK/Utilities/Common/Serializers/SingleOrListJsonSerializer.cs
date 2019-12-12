using Newtonsoft.Json;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    internal class SingleOrListJsonSerializer<T> : DefaultJsonSerializer
    {
        private JsonSerializerSettings settings;
        protected override JsonSerializerSettings Settings => settings ?? (settings = CreateSettings());

        private JsonSerializerSettings CreateSettings()
        {
            var settings = base.Settings;
            settings.Converters.Add(new SingleOrListConverter<T>());
            return settings;
        }
    }
}
