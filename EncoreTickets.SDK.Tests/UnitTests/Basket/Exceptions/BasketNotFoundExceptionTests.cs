using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Basket.Exceptions;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Basket.Exceptions
{
    internal class BasketNotFoundExceptionTests
    {
        [Test]
        public void Constructor_InitializesCorrectly()
        {
            var response = new RestResponse();
            var context = new ApiContext();
            var contextInResponse = new Context();
            var requestInResponse = new Request();
            var sourceException = new ApiException(response, context, contextInResponse, requestInResponse);
            var basketId = "123456789";

            var result = new BasketNotFoundException(sourceException, basketId);

            Assert.IsInstanceOf<ApiException>(result);
            Assert.AreEqual(basketId, result.BasketId);
            Assert.AreEqual(sourceException.Response, result.Response);
            Assert.AreEqual(sourceException.Context, result.Context);
            Assert.AreEqual(sourceException.ContextInResponse, result.ContextInResponse);
            Assert.AreEqual(sourceException.RequestInResponse, result.RequestInResponse);
            Assert.AreEqual(sourceException.ResponseCode, result.ResponseCode);
            Assert.AreEqual(sourceException.Message, result.Message);
            Assert.AreEqual(sourceException.Errors, result.Errors);
        }
    }
}
