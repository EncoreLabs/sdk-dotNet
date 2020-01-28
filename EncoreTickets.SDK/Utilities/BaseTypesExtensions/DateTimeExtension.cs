using System;
using EncoreTickets.SDK.Utilities.BusinessHelpers;

namespace EncoreTickets.SDK.Utilities.BaseTypesExtensions
{
    /// <summary>
    /// The extensions for DateTime class.
    /// </summary>
    public static class DateTimeExtension
    {
        public const string CompressedEncoreDateFormat = "yyyyMMdd";
        public const string CompressedEncoreTimeFormat = "HHmm";
        public const string ReadableEncoreDateFormat = "yyyy-MM-dd";
        public const string ReadableEncoreTimeFormat = "HH:mm";

        /// <summary>
        /// Returns the date in a compressed Encore format.
        /// </summary>
        /// <param name="dateTime">The date.</param>
        /// <returns>The formatted date.</returns>
        public static string ToEncoreDate(this DateTime dateTime)
            => dateTime.ToString(CompressedEncoreDateFormat);

        /// <summary>
        /// Return the time in a compressed Encore format.
        /// </summary>
        /// <param name="dateTime">The time.</param>
        /// <returns>The formatted time.</returns>
        public static string ToEncoreTime(this DateTime dateTime)
            => dateTime.ToString(CompressedEncoreTimeFormat);

        /// <summary>
        /// Returns the date in a readable Encore format.
        /// </summary>
        /// <param name="dateTime">The date.</param>
        /// <returns>The formatted date.</returns>
        public static string ToReadableEncoreDate(this DateTime dateTime)
            => dateTime.ToString(ReadableEncoreDateFormat);

        /// <summary>
        /// Returns the time in a readable Encore format.
        /// </summary>
        /// <param name="dateTime">The time.</param>
        /// <returns>The formatted time.</returns>
        public static string ToReadableEncoreTime(this DateTime dateTime)
            => dateTime.ToString(ReadableEncoreTimeFormat);

        /// <summary>
        /// Returns the performance type (matinee or evening) based on a date in string format.
        /// </summary>
        /// <param name="date">The performance date.</param>
        /// <param name="referenceHour">The hour value that divides the time of day.</param>
        /// <returns>Performance type in string format.</returns>
        public static string GetPerformanceType(this DateTime date, int referenceHour)
            => PerformanceHelper.GetPerformanceType(date, referenceHour);

        /// <summary>
        /// Resets seconds in date.
        /// </summary>
        /// <param name="date">The source date.</param>
        /// <returns>Date with empty seconds.</returns>
        public static DateTime StripSeconds(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
        }

        /// <summary>
        /// Returns the last day of the month.
        /// </summary>
        /// <param name="date">The date of interest</param>
        /// <returns>The last day of the month</returns>
        public static int GetLastDayOfMonth(this DateTime date)
        {
            return GetLastDayOfMonth(date.Month, date.Year);
        }

        /// <summary>
        /// Returns the last day of the month.
        /// </summary>
        /// <param name="month">The month of interest</param>
        /// <param name="year">The year of interest</param>
        /// <returns>The last day of the month</returns>
        public static int GetLastDayOfMonth(int month, int year)
        {
            try
            {
                return DateTime.DaysInMonth(year, month);
            }
            catch (ArgumentOutOfRangeException)
            {
                return 0;
            }
        }
    }
}
