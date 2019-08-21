using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiResultTests
    {
        private static readonly object TestObject = new object();

        private static readonly object[] SourceForConstructorTest =
        {
            new object[]
            {
                new RestResponse {ResponseStatus = ResponseStatus.Completed},
                new ApiResponse<string>("test"),
                false,
                null
            },
            new object[]
            {
                new RestResponse {ResponseStatus = ResponseStatus.Completed, StatusCode = HttpStatusCode.OK},
                new ApiResponse<string>("test"),
                true,
                "test"
            },
            new object[]
            {
                new RestResponse {ResponseStatus = ResponseStatus.Completed},
                new ApiResponse<object>(TestObject),
                false,
                null
            },
            new object[]
            {
                new RestResponse {ResponseStatus = ResponseStatus.Completed, StatusCode = HttpStatusCode.OK},
                new ApiResponse<object>(TestObject),
                true,
                TestObject
            },
            new object[]
            {
                new RestResponse { ResponseStatus = ResponseStatus.Aborted},
                new ApiResponse<string>("test"),
                false,
                null
            },
            new object[]
            {
                new RestResponse { ResponseStatus = ResponseStatus.Error },
                new ApiResponse<string>("test"),
                false,
                null
            },
            new object[]
            {
                new RestResponse { ResponseStatus = ResponseStatus.None },
                new ApiResponse<string>("test"),
                false,
                null
            },
            new object[]
            {
                new RestResponse { ResponseStatus = ResponseStatus.TimedOut },
                new ApiResponse<string>("test"),
                false,
                null
            },
        };

        [TestCaseSource(nameof(SourceForConstructorTest))]
        public void Api_ApiResult_Constructor_InitializesProperties<T>(IRestResponse response, ApiResponse<T> data,
            bool expectedResult, T expectedData)
            where T : class
        {
            var context = It.IsAny<ApiContext>();
            var result = new ApiResult<T>(context, response, data);
            Assert.AreEqual(context, result.Context);
            Assert.AreEqual(expectedResult, result.Result);
            Assert.AreEqual(expectedData, result.Data);
        }
    }
}
