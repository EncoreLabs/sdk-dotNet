using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EncoreTickets.SDK.Api.Results.Response
{
    /// <summary>
    /// The entity for collecting items received with additional information for pagination.
    /// </summary>
    /// <typeparam name="T">Model type</typeparam>
    [JsonObject]
    public class ResponseForPage<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets or sets the collection of received items.
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the number of all existing pages.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the number of items on this pages.
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Gets or sets the number of all existing items.
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
