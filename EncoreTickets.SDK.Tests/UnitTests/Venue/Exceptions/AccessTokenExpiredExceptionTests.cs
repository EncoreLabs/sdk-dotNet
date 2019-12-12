using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Venue.Exceptions;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Venue.Exceptions
{
    internal class AccessTokenExpiredExceptionTests
    {
        [Test]
        public void Constructor_InitializesCorrectly()
        {
            var expectedMessage = "Your token is invalid, please login again to get a new one";
            var expectedResponseCode = HttpStatusCode.Forbidden;
            var response = new RestResponse();
            var context = new ApiContext();
            var contextInResponse = new Context();
            var requestInResponse = new Request();
            var sourceException = new ApiException(response, context, contextInResponse, requestInResponse);

            var result = new AccessTokenExpiredException(sourceException);

            Assert.IsInstanceOf<ApiException>(result);
            Assert.AreEqual(sourceException.Response, result.Response);
            Assert.AreEqual(sourceException.Context, result.Context);
            Assert.AreEqual(sourceException.ContextInResponse, result.ContextInResponse);
            Assert.AreEqual(sourceException.RequestInResponse, result.RequestInResponse);
            Assert.AreEqual(expectedResponseCode, result.ResponseCode);
            Assert.AreEqual(expectedMessage, result.Message);
            Assert.AreEqual(sourceException.Errors, result.Errors);
        }
    }
}
