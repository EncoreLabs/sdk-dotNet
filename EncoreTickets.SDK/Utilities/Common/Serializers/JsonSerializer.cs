using System.IO;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    abstract class JsonSerializer : ISerializer, IDeserializer
    {
        protected abstract Newtonsoft.Json.JsonSerializer Serializer { get; }

        public static T CreateInstance<T>(string dateFormat = null) 
            where T : JsonSerializer, new()
        {
            var serializer = new T();
            if (!string.IsNullOrEmpty(dateFormat))
            {
                serializer.Serializer.DateFormatString = dateFormat;
            }
            return serializer;
        }

        public string ContentType { get; set; } = "application/json";

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            using (var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
                Serializer.Serialize(jsonTextWriter, obj);
                return stringWriter.ToString();
            }
        }

        public T Deserialize<T>(IRestResponse response)
        {
            var content = response.Content;
            using (var stringReader = new StringReader(content))
            using (var jsonTextReader = new JsonTextReader(stringReader))
            {
                return Serializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}
