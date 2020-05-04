using System;
using System.Linq;

namespace EncoreTickets.SDK.Utilities.CommonModels.Extensions
{
    public static class EntityWithStatusExtension
    {
        public static bool HasStatus(this IEntityWithStatus item, string status)
        {
            return item?.Status != null && CompareStatuses(item.Status, status);
        }

        public static bool HasOneOfStatuses(this IEntityWithStatus item, params string[] statuses)
        {
            return item?.Status != null && statuses.Any(x => CompareStatuses(item.Status, x));
        }

        internal static bool CompareStatuses(string sourceStatus, string status)
        {
            return sourceStatus != null && sourceStatus.Equals(status, StringComparison.OrdinalIgnoreCase);
        }
    }
}
