using System.Collections.Generic;
using System.Net;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiResultListTests
    {
        private static readonly object[] SourceForConstructorTest =
        {
            new object[]
            {
                new RestResponse {ResponseStatus = ResponseStatus.Completed},
                new ApiResponse<string>("test"),
                true,
                0
            },
            new object[]
            {
                new RestResponse {ResponseStatus = ResponseStatus.Completed, StatusCode = HttpStatusCode.OK},
                new ApiResponse<object>(new object()),
                true,
                0
            },
            new object[]
            {
                new RestResponse {ResponseStatus = ResponseStatus.Completed},
                new ApiResponse<IEnumerable<object>>(new List<object>{new object()}),
                true,
                0
            },
            new object[]
            {
                new RestResponse {ResponseStatus = ResponseStatus.Completed, StatusCode = HttpStatusCode.OK},
                new ApiResponse<IEnumerable<object>>(new List<object>{new object()}),
                true,
                0
            },
            new object[]
            {
                new RestResponse {ResponseStatus = ResponseStatus.Completed, StatusCode = HttpStatusCode.OK},
                new ApiResponse<IEnumerable<TestObject>>(new List<TestObject>{new TestObject()}),
                true,
                1
            },
            new object[]
            {
                new RestResponse { ResponseStatus = ResponseStatus.Aborted},
                new ApiResponse<object>(new object()),
                false,
                0
            },
            new object[]
            {
                new RestResponse { ResponseStatus = ResponseStatus.Error },
                new ApiResponse<string>("test"),
                false,
                0
            },
            new object[]
            {
                new RestResponse { ResponseStatus = ResponseStatus.None },
                new ApiResponse<object>(new object()),
                false,
                0
            },
            new object[]
            {
                new RestResponse { ResponseStatus = ResponseStatus.TimedOut },
                new ApiResponse<object>(new object()),
                false,
                0
            },
        };

        [TestCaseSource(nameof(SourceForConstructorTest))]
        public void ApiResultList_Constructor_InitializesProperties<T>(IRestResponse response, ApiResponse<T> data,
            bool expectedResult, int expectedCount)
            where T : class
        {
            var context = It.IsAny<ApiContext>();
            var result = new ApiResultList<T>(context, It.IsAny<IRestRequest>(), response, data);
            Assert.AreEqual(context, result.Context);
            Assert.AreEqual(expectedResult, result.Result);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [Test]
        public void ApiResultList_GetList_ReturnsList()
        {
            var response = new RestResponse {ResponseStatus = ResponseStatus.Completed, StatusCode = HttpStatusCode.OK};
            var data = new ApiResponse<TestObject[]>(new []{new TestObject(), new TestObject()});
            var resultList = new ApiResultList<TestObject[]>(It.IsAny<ApiContext>(), It.IsAny<IRestRequest>(), response, data);
            var result = resultList.GetList<TestObject>();
            Assert.IsTrue(result != null);
            Assert.AreEqual(2, result.Count);
        }
    }
}
