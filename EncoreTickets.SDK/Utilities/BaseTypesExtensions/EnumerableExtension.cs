using System;
using System.Collections.Generic;
using System.Linq;

namespace EncoreTickets.SDK.Utilities.BaseTypesExtensions
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

        /// <summary>
        /// Returns new collection where there are no null or empty strings. 
        /// </summary>
        /// <param name="stringCollection">Source string collection.</param>
        /// <returns>Enumerable without empty strings.</returns>
        public static IEnumerable<string> ExcludeEmptyStrings(this IEnumerable<string> stringCollection)
        {
            return stringCollection?.Where(x => !string.IsNullOrEmpty(x));
        }

        /// <summary>
        /// Returns null if source enumerable has no elements, otherwise returns the source enumerable. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">Source enumerable.</param>
        /// <returns>Null or the source enumerable</returns>
        public static List<T> NullIfEmptyEnumerable<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any() ? null : enumerable.ToList();
        }
    }
}
