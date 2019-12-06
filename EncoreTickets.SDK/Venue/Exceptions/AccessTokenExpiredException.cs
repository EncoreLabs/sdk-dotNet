using System.Net;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Exceptions;

namespace EncoreTickets.SDK.Venue.Exceptions
{
    /// <summary>
    /// The exception if a request to the venue API failed because the access token is expired.
    /// </summary>
    public class AccessTokenExpiredException : ApiException
    {
        public override string Message => "Your token is invalid, please login again to get a new one";

        public override HttpStatusCode ResponseCode => HttpStatusCode.Forbidden;

        public ApiException SourceException { get; }

        internal AccessTokenExpiredException(ApiException exception) : base(exception)
        {
            SourceException = exception;
        }
    }
}
