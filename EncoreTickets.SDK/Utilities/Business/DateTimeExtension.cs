using System;

namespace EncoreTickets.SDK.Utilities.Business
{
    /// <summary>
    /// The extensions for DateTime class.
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// Returns the date in Encore string format.
        /// </summary>
        /// <param name="dateTime">The date.</param>
        /// <returns>The formatted date.</returns>
        public static string ToEncoreDate(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMdd");
        }

        /// <summary>
        /// Return the time in Encore string format.
        /// </summary>
        /// <param name="dateTime">The time.</param>
        /// <returns>The formatted time.</returns>
        public static string ToEncoreTime(this DateTime dateTime)
        {
            return dateTime.ToString("HHmm");
        }
    }
}
