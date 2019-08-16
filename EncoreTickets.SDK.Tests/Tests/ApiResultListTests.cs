using System.Collections.Generic;
using System.Net;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.Tests
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
                new ApiResponse<IEnumerable<TestObject1>>(new List<TestObject1>{new TestObject1()}),
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
            var data = new ApiResponse<TestObject1[]>(new []{new TestObject1(), new TestObject1()});
            var resultList = new ApiResultList<TestObject1[]>(It.IsAny<ApiContext>(), It.IsAny<IRestRequest>(), response, data);
            var result = resultList.GetList<TestObject1>();
            Assert.IsTrue(result != null);
            Assert.AreEqual(2, result.Count);
        }
    }
}
