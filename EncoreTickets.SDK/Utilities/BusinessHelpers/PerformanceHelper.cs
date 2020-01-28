using System;

namespace EncoreTickets.SDK.Utilities.BusinessHelpers
{
    public static class PerformanceHelper
    {
        public static string GetPerformanceType(DateTime date, int referenceHour)
        {
            const string eveningType = "E";
            const string matineeType = "M";
            return date.Hour >= referenceHour ? eveningType : matineeType;
        }
    }
}
