using System.Collections.Generic;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
using Moq;
using NUnit.Framework;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiResultTests
    {
        private const string CodeOfInfoAsError = "notValidPromotionCode";

        private static Context[] SourceForGetDataOrContextException_ReturnsData =
        {
            null,
            new Context(),
            new Context {info = new List<Info>()},
            new Context {info = new List<Info> {new Info {code = "information"}}},
        };

        [Test]
        public void Api_ApiResult_ConstructorWith5Args_IfSuccessfulResponse_InitializesCommonProperties()
        {
            var data = new[] {new object(), new object(),};
            var response = TestHelper.GetSuccessResponse();
            var responseContext = new Context();
            var requestInResponse = new Request();
            var context = It.IsAny<ApiContext>();

            var result = new ApiResult<object[]>(data, response, context, responseContext, requestInResponse);

            Assert.AreEqual(context, result.Context);
            Assert.AreEqual(response, result.RestResponse);
            Assert.AreEqual(responseContext, result.ResponseContext);
            Assert.AreEqual(requestInResponse, result.RequestInResponse);
            Assert.AreEqual(true, result.IsSuccessful);
            Assert.AreEqual(default, result.ApiException);
            Assert.AreEqual(data, result.DataOrException);
            Assert.AreEqual(data, result.DataOrDefault);
        }

        [Test]
        public void Api_ApiResult_ConstructorWith5Args_IfUnsuccessfulResponse_InitializesCommonProperties()
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
            Assert.AreEqual(thrownException, result.ApiException);
        }

        [Test]
        public void Api_ApiResult_ConstructorWith4Args_IfNullError_DoesNotInitializeApiException()
        {
            var response = TestHelper.GetSuccessResponse();
            var data = new[] { new object(), new object(), };
            string error = null;

            var result = new ApiResult<object[]>(data, response, It.IsAny<ApiContext>(), error);

            Assert.AreEqual(default, result.ApiException);
            Assert.AreEqual(data, result.DataOrException);
        }

        [Test]
        public void Api_ApiResult_ConstructorWith4Args_IfNotNullError_InitializesApiException()
        {
            var response = TestHelper.GetFailedResponse();
            object[] data = null;
            var error = "Error";

            var result = new ApiResult<object[]>(data, response, It.IsAny<ApiContext>(), error);

            var thrownException = Assert.Catch<ApiException>(() =>
            {
                var resultData = result.DataOrException;
            });
            Assert.AreEqual(thrownException, result.ApiException);
            Assert.AreEqual(error, result.ApiException.Message);
        }

        [Test]
        public void Api_ApiResult_ConstructorWith4Args_IfUnsuccessfulResponse_InitializesCommonProperties()
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
            Assert.AreEqual(thrownException, result.ApiException);
        }

        [TestCaseSource(nameof(SourceForGetDataOrContextException_ReturnsData))]
        public void Api_ApiResult_GetDataOrContextException_IfContextWithoutInfosAsErrors_ReturnsData(Context responseContext)
        {
            var infosAsErrors = new[] { CodeOfInfoAsError };
            var data = new[] { new object(), new object(), };
            var response = TestHelper.GetSuccessResponse();

            var result = new ApiResult<object[]>(data, response, It.IsAny<ApiContext>(), responseContext, It.IsAny<Request>());

            Assert.AreEqual(data, result.GetDataOrContextException(infosAsErrors));
        }

        [Test]
        public void Api_ApiResult_GetDataOrContextException_IfContextWithInfosAsErrors_ThrowsException()
        {
            var infosAsErrors = new[] { CodeOfInfoAsError };
            var data = new[] { new object(), new object(), };
            var response = TestHelper.GetSuccessResponse();
            var responseContext = new Context {info = new List<Info> {new Info {code = CodeOfInfoAsError } }};

            var result = new ApiResult<object[]>(data, response, It.IsAny<ApiContext>(), responseContext, It.IsAny<Request>());

            Assert.Catch<ContextApiException>(() => result.GetDataOrContextException(infosAsErrors));
        }
    }
}
