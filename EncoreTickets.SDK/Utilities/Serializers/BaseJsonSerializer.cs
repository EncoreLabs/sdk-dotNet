using EncoreTickets.SDK.Utilities.Enums;
using Newtonsoft.Json;
using RestSharp;
using DataFormat = EncoreTickets.SDK.Utilities.Enums.DataFormat;

namespace EncoreTickets.SDK.Utilities.Serializers
{
    public abstract class BaseJsonSerializer : ISerializerWithDateFormat, ISerializer, IDeserializer
    {
        public DataFormat SerializedDataFormat => DataFormat.Json;

        public JsonSerializerSettings Settings { get; }

        public string ContentType { get; set; } // Required by RestSharp

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

        protected BaseJsonSerializer(JsonSerializerSettings settings)
        {
            ContentType = DataFormatHelper.ToContentType(SerializedDataFormat);
            Settings = settings;
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }

        public T Deserialize<T>(IRestResponse response)
        {
            var content = response.Content;
            return Deserialize<T>(content);
        }

        public T Deserialize<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content, Settings);
        }
    }
}
