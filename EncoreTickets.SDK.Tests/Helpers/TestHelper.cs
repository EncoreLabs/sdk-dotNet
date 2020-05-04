using System;
using System.Globalization;
using Newtonsoft.Json;

namespace EncoreTickets.SDK.Tests.Helpers
{
    internal static class TestHelper
    {
        public static CultureInfo Culture = new CultureInfo("en");

        public static DateTime ConvertTestArgumentToDateTime(string arg)
        {
            return DateTime.Parse(arg, Culture);
        }

        public static T CopyObject<T>(T value)
        {
            var output = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(output);
        }
    }
}
