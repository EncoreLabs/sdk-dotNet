using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EncoreTickets.SDK.Content
{
    public class LocationResponse : IEnumerable<IObject>
    {
        [DataMember]
        public List<Location> locations { get; set; }

        /// <summary>
        /// REturn the data
        /// </summary>
        public List<IObject> Data { get { return this.locations.ConvertAll<IObject>(p => p as IObject); } }

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
}
