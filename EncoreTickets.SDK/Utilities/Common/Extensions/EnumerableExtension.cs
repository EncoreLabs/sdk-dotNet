using System.Collections.Generic;
using System.Linq;

namespace EncoreTickets.SDK.Utilities.Common.Extensions
{
    internal static class EnumerableExtension
    {
        public static IEnumerable<string> ExcludeEmptyStrings(this IEnumerable<string> stringCollection)
        {
            return stringCollection?.Where(x => !string.IsNullOrEmpty(x));
        }

        public static List<T> NullIfEmptyEnumerable<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any() ? null : enumerable.ToList();
        }
    }
}
