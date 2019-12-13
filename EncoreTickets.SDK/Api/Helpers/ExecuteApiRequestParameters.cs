using EncoreTickets.SDK.Utilities.Common.Serializers;
using EncoreTickets.SDK.Utilities.Enums;

namespace EncoreTickets.SDK.Api.Helpers
{
    public class ExecuteApiRequestParameters
    {
        /// <summary>
        /// Gets or sets API resource endpoint
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets request method
        /// </summary>
        public RequestMethod Method { get; set; }

        /// <summary>
        /// Gets or sets request body
        /// </summary>
        public object Body { get; set; } = null;

        /// <summary>
        /// Gets or sets object for request query
        /// </summary>
        public object Query { get; set; } = null;

        /// <summary>
        /// Gets or sets request date format
        /// </summary>
        public string DateFormat { get; set; } = null;

        public ISerializerWithDateFormat Serializer { get; set; } = new DefaultJsonSerializer();

        public ISerializerWithDateFormat Deserializer { get; set; } = new DefaultJsonSerializer();
    }
}