using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    internal abstract class JsonSerializer : ISerializer, IDeserializer
    {
        protected abstract JsonSerializerSettings Settings { get; }

        public static T CreateInstance<T>(string dateFormat = null) 
            where T : JsonSerializer, new()
        {
            var serializer = new T();
            if (!string.IsNullOrEmpty(dateFormat))
            {
                serializer.Settings.DateFormatString = dateFormat;
            }
            return serializer;
        }

        public string ContentType { get; set; } = "application/json"; // Required by RestSharp

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
