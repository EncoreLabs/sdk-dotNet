using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Net;
using RestSharp;

namespace EncoreTickets.SDK
{
    [DataContract]
    public abstract class ApiResultBase<T>
    {
        #region Private Members
        
        private IRestRequest request;
        private IRestResponse response;
        private ApiContext context;

        private bool success;

        #endregion // Private Members

        #region Constructors
  
        /// <summary>
        /// Initilaise a new instance of the context object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        protected ApiResultBase(ApiContext context, IRestRequest request, IRestResponse response)
        {
            this.request = request;
            this.response = response;
            this.success = response.ResponseStatus == ResponseStatus.Completed;
            this.context = context;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this call was a success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Result
        {
            get { return this.success; }
        }

        /// <summary>
        /// Context object
        /// </summary>
        public ApiContext Context
        {
            get { return this.context; }
        }

        #endregion // Properties
    }
}
