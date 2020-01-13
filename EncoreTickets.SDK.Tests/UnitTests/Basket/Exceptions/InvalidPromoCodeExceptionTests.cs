using System.Collections.Generic;
using EncoreTickets.SDK.Api.Models;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using EncoreTickets.SDK.Basket.Exceptions;
using EncoreTickets.SDK.Basket.Models;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Basket.Exceptions
{
    internal class InvalidPromoCodeExceptionTests
    {
        [Test]
        public void Constructor_InitializesCorrectly()
        {
            var contextErrors = new List<Info>();
            var response = new RestResponse();
            var context = new ApiContext();
            var contextInResponse = new Context();
            var requestInResponse = new Request();
            var sourceException = new ContextApiException(contextErrors, response, context, contextInResponse, requestInResponse);
            var coupon = new Coupon();

            var result = new InvalidPromoCodeException(sourceException, coupon);

            Assert.IsInstanceOf<ApiException>(result);
            Assert.IsInstanceOf<ContextApiException>(result);
            Assert.AreEqual(coupon, result.Coupon);
            Assert.AreEqual(sourceException.ContextErrors, result.ContextErrors);
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
