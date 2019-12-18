using EncoreTickets.SDK.Api.Helpers;
using Newtonsoft.Json;
using RestSharp;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    public abstract class BaseJsonSerializer : ISerializerWithDateFormat
    {
        public string ContentType { get; set; } = ContentTypes.ApplicationJson; // Required by RestSharp

        public string DateFormat
        {
            get => Settings.DateFormatString;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Settings.DateFormatString = value;
                }
            }
        }

        protected JsonSerializerSettings Settings { get; }

        protected BaseJsonSerializer(JsonSerializerSettings settings)
        {
            Settings = settings;
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
