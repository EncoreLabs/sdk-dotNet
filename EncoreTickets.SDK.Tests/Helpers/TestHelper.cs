using System;
using System.Globalization;

namespace EncoreTickets.SDK.Tests.Helpers
{
    internal static class TestHelper
    {
        public static CultureInfo Culture = new CultureInfo("en");

        public static DateTime ConvertTestArgumentToDateTime(string arg)
        {
            return arg == null ? default : DateTime.Parse(arg, Culture);
        }
    }
}
