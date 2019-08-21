using System.Collections.Generic;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Interfaces;
using RestSharp;

namespace EncoreTickets.SDK.Api.Results
{
    /// <summary>
    /// Class representing enumerable result of Api call.
    /// </summary>
    /// <typeparam name="T">Enumerable data type</typeparam>
    public class ApiResultList<T> : ApiResultBase
        where T : IEnumerable<IObject>
    {
        private readonly List<IObject> items;

        /// <summary>
        /// Gets the number of result items.
        /// </summary>
        public int Count => items.Count;

        /// <summary>
        /// Initializes a new instance of <see cref="ApiResultList"/>
        /// </summary>
        /// <param name="context">Api context.</param>
        /// <param name="response">Response.</param>
        /// <param name="data">Response data.</param>
        public ApiResultList(ApiContext context, IRestResponse response, ApiResponse<T> data) :
            base(context, response)
        {
            if (response.IsSuccessful && data.Data is IEnumerable<IObject> enumerable)
            {
                items = new List<IObject>(enumerable);
            }
            else
            {
                items = new List<IObject>();
            }
        }

        /// <summary>
        /// Get the list of result items.
        /// </summary>
        /// <returns>Result items.</returns>
        public IList<TObject> GetList<TObject>()
            where TObject : IObject
        {
            return items.ConvertAll(i => (TObject) i);
        }
    }
}