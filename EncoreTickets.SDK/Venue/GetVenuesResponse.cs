using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EncoreTickets.SDK.Venue
{
    public class GetVenuesResponse : IEnumerable<IObject>
    {
        [DataMember]
        public Response response { get; set; }

        /// <summary>
        /// REturn the data
        /// </summary>
        public List<IObject> Data { get { return response.results.ConvertAll(p => p as IObject); } }

        /// <summary>
        /// Return the enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IObject> GetEnumerator()
        {
            return this.Data.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Data.GetEnumerator();
        }
    }

    public class Response
    {
        public List<Venue> results { get; set; }
    }
}