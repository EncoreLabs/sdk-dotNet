using RestSharp;
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
        private readonly List<IObject> items;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public ApiResultList(ApiContext context, IRestRequest request, IRestResponse response, ApiResponse<T> data) : 
            base(context, request, response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK && data.Data is IEnumerable<IObject> enumerable)
            {
                items = new List<IObject>(enumerable);
            }
            else
            {
                items = new List<IObject>();
            }
        }

        /// <summary>
        /// Gets the number of items in the colleciton
        /// </summary>
        public int Count => items.Count;

        /// <summary>
        /// Get the list of items
        /// </summary>
        /// <returns></returns>
        public IList<K> GetList<K>()
            where K : IObject
        {
            return items.ConvertAll(i => (K)i);
        }
    }
}