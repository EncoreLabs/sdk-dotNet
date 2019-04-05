using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using RestSharp;

namespace EncoreTickets.SDK
{
    /// <summary>
    /// Class respresenting result of Api call
    /// </summary>
    /// <typeparam name="T">data type</typeparam>
    [DataContract]
    public class ApiResult<T> : ApiResultBase<T> where T: class
    {
        #region Private Members

        private readonly T data;

        #endregion // Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public ApiResult(ApiContext context, IRestRequest request, IRestResponse response, ApiResponse<T> data) : 
            base(context, request, response)
        {
            if (response.IsSuccessful)
            {
                this.data = data.Data as T;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The data returned by the API response
        /// </summary>
        public T Data
        {
            get { return this.data; }
        }

        #endregion
   
    }
}
