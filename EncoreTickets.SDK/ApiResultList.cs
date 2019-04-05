using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EncoreTickets.SDK
{
    [DataContract]
    public class ApiResultList<T> : ApiResultBase<T>
    {
        /// <summary>
        /// list of results
        /// </summary>
        private List<IObject> items;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public ApiResultList(ApiContext context, IRestRequest request, IRestResponse response, ApiResponse<T> data) : 
            base(context, request, response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK && data.Data is IEnumerable)
            {
                IEnumerable<IObject> enumerable = data.Data as IEnumerable<IObject>;
                this.items = new List<IObject>(enumerable);
            }
            else
            {
                this.items = new List<IObject>();
            }
        }

        /// <summary>
        /// Gets the number of items in the colleciton
        /// </summary>
        public int Count
        {
            get { return this.items.Count; }
        }

        /// <summary>
        /// Get the list of items
        /// </summary>
        /// <returns></returns>
        public IList<K> GetList<K>() where K : IObject
        {
            return this.items.ConvertAll<K>(i => (K)i);
        }
    }
}