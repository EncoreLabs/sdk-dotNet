using System.Collections;
using System.Collections.Generic;
using EncoreTickets.SDK.Interfaces;

namespace EncoreTickets.SDK.Api.Results
{
    /// <summary>
    /// The entity for collecting items received with additional information for pagination.
    /// </summary>
    /// <typeparam name="T">Model type</typeparam>
    public class ResponseForPage<T> : IEnumerable<T>
        where T : IObject
    {
        /// <summary>
        /// Gets or sets the collection of received items.
        /// </summary>
        public List<T> items { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int currentPage { get; set; }

        /// <summary>
        /// Gets or sets the number of all existing pages.
        /// </summary>
        public int pageCount { get; set; }

        /// <summary>
        /// Gets or sets the number of items on this pages.
        /// </summary>
        public int itemsPerPage { get; set; }

        /// <summary>
        /// Gets or sets the number of all existing items.
        /// </summary>
        public int totalItemCount { get; set; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
