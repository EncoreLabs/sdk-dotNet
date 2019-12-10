using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace EncoreTickets.SDK.Utilities.Common.Serializers
{
    class JsonSerializer : ISerializer, IDeserializer
    {
        private readonly Newtonsoft.Json.JsonSerializer defaultSerializer = new Newtonsoft.Json.JsonSerializer
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly Newtonsoft.Json.JsonSerializer serializer;

        public JsonSerializer()
        {
            serializer = defaultSerializer;
        }

        public JsonSerializer(string dateFormat)
        {
            serializer = defaultSerializer;
            serializer.DateFormatString = dateFormat;
        }

        public JsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public string ContentType { get; set; } = "application/json";

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            using(var jsonTextWriter = new JsonTextWriter(stringWriter))
            {
                serializer.Serialize(jsonTextWriter, obj);
                return stringWriter.ToString();
            }
        }

        public T Deserialize<T>(IRestResponse response)
        {
            var content = response.Content;
            using (var stringReader = new StringReader(content))
            using (var jsonTextReader = new JsonTextReader(stringReader))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}
