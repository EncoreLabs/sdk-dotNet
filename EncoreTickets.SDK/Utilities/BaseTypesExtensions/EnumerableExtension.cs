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

        public static IEnumerable<string> ExcludeEmptyStrings(this IEnumerable<string> stringCollection)
        {
            return stringCollection?.Where(x => !string.IsNullOrEmpty(x));
        }

        public static List<T> NullIfEmptyEnumerable<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any() ? null : enumerable.ToList();
        }

        internal static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T itemToPrepend)
        {
            yield return itemToPrepend;
            foreach (var item in source)
            {
                yield return item;
            }
        }
    }
}
