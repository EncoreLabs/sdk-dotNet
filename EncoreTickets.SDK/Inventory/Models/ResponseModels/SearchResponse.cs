using System.Collections;
using System.Collections.Generic;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Inventory.Models.ResponseModels
{
    internal class SearchResponse : IEnumerable<IObject>
    {
        /// <summary>
        /// Returns the data.
        /// </summary>
        public List<IObject> Data => product.ConvertAll(p => p as IObject);
        
        public List<Product> product { get; set; }

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
