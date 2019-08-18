using System.Collections.Generic;
using System.Runtime.Serialization;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Interfaces;
using RestSharp;

namespace EncoreTickets.SDK.Api.Results
{
    [DataContract]
    public class ApiResultList<T> : ApiResultBase<T>
    {
        /// <summary>
        /// list of results
        /// </summary>
        private readonly List<IObject> items;

        /// <summary>
        /// Gets the number of items in the collection.
        /// </summary>
        public int Count => items.Count;

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
        /// Get the list of items
        /// </summary>
        /// <returns></returns>
        public IList<TObject> GetList<TObject>()
            where TObject : IObject
        {
            return items.ConvertAll(i => (TObject)i);
        }
    }
}