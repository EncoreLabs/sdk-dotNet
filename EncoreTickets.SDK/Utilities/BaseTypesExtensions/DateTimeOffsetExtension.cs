using System;
using EncoreTickets.SDK.Utilities.BusinessHelpers;

namespace EncoreTickets.SDK.Utilities.BaseTypesExtensions
{
    /// <summary>
    /// The extensions for DateTimeOffset class.
    /// </summary>
    public static class DateTimeOffsetExtension
    {
        /// <summary>
        /// Returns the date in a readable Encore format.
        /// </summary>
        /// <param name="dateTime">The date.</param>
        /// <returns>The formatted date.</returns>
        public static string ToReadableEncoreDate(this DateTimeOffset dateTime)
            => dateTime.DateTime.ToReadableEncoreDate();

        /// <summary>
        /// Returns the time in a readable Encore format.
        /// </summary>
        /// <param name="dateTime">The time.</param>
        /// <returns>The formatted time.</returns>
        public static string ToReadableEncoreTime(this DateTimeOffset dateTime)
            => dateTime.DateTime.ToReadableEncoreTime();

        /// <summary>
        /// Returns the performance type (matinee or evening) based on a date in string format.
        /// </summary>
        /// <param name="dateTime">The performance time.</param>
        /// <param name="referenceHour">The hour value that divides the time of day.</param>
        /// <returns>Performance type in string format.</returns>
        public static string GetPerformanceType(this DateTimeOffset dateTime, int referenceHour)
            => PerformanceHelper.GetPerformanceType(dateTime.DateTime, referenceHour);
    }
}