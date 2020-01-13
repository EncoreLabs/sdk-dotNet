using System;
using System.Collections.Generic;
using System.Linq;

namespace EncoreTickets.SDK.Utilities.BaseTypesExtensions
{
    public static class EnumExtension
    {
        public static List<T> GetListWithAllEnumValues<T>()
            where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}