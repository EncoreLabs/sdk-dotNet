using System;
using System.Globalization;
using System.Threading;

namespace EncoreTickets.SDK.Tests
{
    internal static class TestHelper
    {
        public static CultureInfo Culture = new CultureInfo("en");

        public static DateTime ConvertTestArgumentToDateTime(string arg)
        {
            return arg == null ? default : DateTime.Parse(arg, Culture);
        }

        public static void SetCultureGlobally(string cultureName)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
        }
    }
}
