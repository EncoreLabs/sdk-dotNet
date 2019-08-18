using System.Runtime.Serialization;
using EncoreTickets.SDK.Api.Context;
using RestSharp;

namespace EncoreTickets.SDK.Api.Results
{
    [DataContract]
    public abstract class ApiResultBase<T>
    {
        private IRestRequest request;
        private IRestResponse response;

        /// <summary>
        /// Gets a value indicating whether this call was a success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Result { get; }

        /// <summary>
        /// Context object
        /// </summary>
        public ApiContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the context object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        protected ApiResultBase(ApiContext context, IRestRequest request, IRestResponse response)
        {
            this.request = request;
            this.response = response;
            Result = response.ResponseStatus == ResponseStatus.Completed;
            Context = context;
        }
    }
}
