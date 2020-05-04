using EncoreTickets.SDK.Utilities.Enums;
using EncoreTickets.SDK.Utilities.Serializers;

namespace EncoreTickets.SDK.Api.Utilities.RequestExecutor
{
    /// <summary>
    /// Parameters to get a result of API execution
    /// </summary>
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
        public object Body { get; set; }

        /// <summary>
        /// Gets or sets object for request query
        /// </summary>
        public object Query { get; set; }

        /// <summary>
        /// Gets or sets request date format
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// Gets or sets JSON serializer for requests
        /// </summary>
        public ISerializerWithDateFormat Serializer { get; set; }

        /// <summary>
        /// Gets or sets JSON deserializer for responses
        /// </summary>
        public ISerializerWithDateFormat Deserializer { get; set; }
    }
}