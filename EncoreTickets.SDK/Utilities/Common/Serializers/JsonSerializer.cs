using Newtonsoft.Json;
using RestSharp;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    public abstract class JsonSerializer : ISerializerWithDateFormat, IDeserializerWithDateFormat
    {
        private readonly JsonSerializerSettings settings;

        public string ContentType { get; set; } = "application/json"; // Required by RestSharp

        protected JsonSerializerSettings Settings => settings;

        public string DateFormat
        {
            set => Settings.DateFormatString = value;
        }

        protected JsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        public T Deserialize<T>(IRestResponse response)
        {
            var content = response.Content;
            return JsonConvert.DeserializeObject<T>(content, Settings);
        }
    }
}
