using System;
using System.Globalization;
using System.Net;
using System.Threading;
using RestSharp;

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

        public static RestResponse GetSuccessResponse()
        {
            return new RestResponse
            {
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = HttpStatusCode.OK
            };
        }

        public static RestResponse GetFailedResponse()
        {
            return new RestResponse
            {
                ResponseStatus = ResponseStatus.Error,
            };
        }
    }
}
