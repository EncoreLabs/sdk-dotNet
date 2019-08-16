using System.Runtime.Serialization;

namespace EncoreTickets.SDK
{
    [DataContract]
    public class ApiResponse<T>
    {
        [DataMember]
        public Request request { get; set; }

        [DataMember]
        public T response { get; set; }

        [DataMember]
        public object context { get; set; }

        public object Data => response;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public ApiResponse()
        {
        }
            
        /// <summary>
        /// Initilias ean API response
        /// </summary>
        /// <param name="data"></param>
        public ApiResponse(T data)
        {
            response = data;
        }
    }
}