using System.Net;

namespace EncoreTickets.SDK.Utilities.BaseTypesExtensions
{
    public static class HttpStatusCodeExtension
    {
        public static bool IsServerError(this HttpStatusCode code)
        {
            return (int) code >= 500;
        }
    }
}