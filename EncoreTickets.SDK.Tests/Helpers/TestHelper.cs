using System;
using System.Globalization;
using Newtonsoft.Json;

namespace EncoreTickets.SDK.Tests.Helpers
{
    internal static class TestHelper
    {
        public static readonly CultureInfo Culture = new CultureInfo("en");

        public static DateTime ConvertTestArgumentToDateTime(string arg)
        {
            return DateTime.Parse(arg, Culture);
        }

        public static T CopyObject<T>(this T value)
        {
            var output = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<T>(output);
        }

        public static TTarget CopyObjectToChildClass<TSource, TTarget>(this TSource value)
            where TTarget : TSource
        {
            var output = JsonConvert.SerializeObject(value);
            return JsonConvert.DeserializeObject<TTarget>(output);
        }
    }
}
