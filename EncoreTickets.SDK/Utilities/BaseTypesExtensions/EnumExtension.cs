using System;
using System.Collections.Generic;
using System.Linq;

namespace EncoreTickets.SDK.Utilities.BaseTypesExtensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// Returns the set of all possible enumeration values.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <returns>The enum values.</returns>
        public static List<T> GetEnumValues<T>()
            where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        /// <summary>
        /// Returns a value of a enumeration by its textual representation.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="strValue">The enum value as text.</param>
        /// <returns>The enum value.</returns>
        public static T GetEnumFromString<T>(string strValue)
            where T : Enum
        {
            return (T)Enum.Parse(typeof(T), strValue, true);
        }
    }
}