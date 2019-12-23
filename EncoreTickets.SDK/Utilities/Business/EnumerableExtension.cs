using System;
using System.Collections.Generic;
using System.Linq;

namespace EncoreTickets.SDK.Utilities.Business
{
    public static class EnumerableExtension
    {
        /// <summary>
        /// Returns new collection where elements have unique property value. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Source collection.</param>
        /// <param name="selector">Selector for a property.</param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> source, Func<T, object> selector) 
            => source?.GroupBy(selector).DefaultIfEmpty().Where(g => g != null).Select(g => g.FirstOrDefault());
    }
}
