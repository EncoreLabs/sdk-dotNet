using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Inventory
{
    public class SearchResponse : IEnumerable<IObject>
    {
        [DataMember]
        public List<Product> product { get; set; }

        /// <summary>
        /// REturn the data
        /// </summary>
        public List<IObject> Data { get { return this.product.ConvertAll<IObject>(p => p as IObject); } }

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
