using System.Collections.Generic;
using System.Linq;

namespace EncoreTickets.SDK.Utilities.Common.TypeExtensions
{
    internal static class EnumerableExtension
    {
        public static IEnumerable<string> ExcludeEmptyStrings(this IEnumerable<string> stringCollection)
        {
            return stringCollection.Where(x => !string.IsNullOrEmpty(x));
        }
    }
}
