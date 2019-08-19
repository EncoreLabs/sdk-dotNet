using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Venue.Models.ResponseModels
{
    internal class VenuesResponse : IEnumerable<IObject>
    {
        [DataMember]
        public Response response { get; set; }

        /// <summary>
        /// Returns the data.
        /// </summary>
        public List<IObject> Data => response.results.ConvertAll(p => p as IObject);

        /// <summary>
        /// Returns the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IObject> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    }

    public class Response
    {
        public List<Venue> results { get; set; }
    }
}