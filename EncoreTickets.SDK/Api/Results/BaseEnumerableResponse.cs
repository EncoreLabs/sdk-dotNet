using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Api.Results
{
    internal class BaseEnumerableResponse<T> : IEnumerable<IObject>
        where T : IObject
    {
        [DataMember]
        public List<T> response { get; set; }

        /// <summary>
        /// Returns the data.
        /// </summary>
        public virtual List<IObject> Data => response.ConvertAll(p => p as IObject);

        /// <summary>
        /// Returns the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IObject> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    }
}
