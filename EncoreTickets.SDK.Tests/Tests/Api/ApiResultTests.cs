using System;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiResultTests
    {
        private static readonly object TestObject = new object();

        private static readonly object[] SourceForConstructor5Test_IfSuccessfulResponse =
        {
            new object[] {},
            TestObject,
            "test"
        };

        [TestCaseSource(nameof(SourceForConstructor5Test_IfSuccessfulResponse))]
        public void Api_ApiResult_ConstructorWith5Args_InitializesCommonPropertiesIfSuccessfulResponse<T>(T data)
            where T : class
        {
            var response = TestHelper.GetSuccessResponse();
            var responseContext = new Context();
            var requestInResponse = new Request();
            var context = It.IsAny<ApiContext>();

            var result = new ApiResult<T>(data, response, context, responseContext, requestInResponse);

            Assert.AreEqual(true, result.IsSuccessful);
            Assert.AreEqual(data, result.DataOrException);
            Assert.AreEqual(data, result.DataOrDefault);
            Assert.AreEqual(context, result.Context);
            Assert.AreEqual(response, result.RestResponse);
            Assert.AreEqual(responseContext, result.ResponseContext);
            Assert.AreEqual(requestInResponse, result.RequestInResponse);
            Assert.AreEqual(default, result.Exception);
        }

        [Test]
        public void Api_ApiResult_ConstructorWith5Args_InitializesCommonPropertiesIfUnsuccessfulResponse()
        {
            var response = TestHelper.GetFailedResponse();
            var responseContext = new Context();
            var requestInResponse = new Request();
            var context = It.IsAny<ApiContext>();

            var result = new ApiResult<object>(null, response, context, responseContext, requestInResponse);

            Assert.AreEqual(context, result.Context);
            Assert.AreEqual(response, result.RestResponse);
            Assert.AreEqual(responseContext, result.ResponseContext);
            Assert.AreEqual(requestInResponse, result.RequestInResponse);
            Assert.AreEqual(false, result.IsSuccessful);
            Assert.AreEqual(default, result.DataOrDefault);
            var thrownException = Assert.Catch<ApiException>(() =>
            {
                var data = result.DataOrException;
            });
            Assert.AreEqual(thrownException, result.Exception);
        }
    }
}
