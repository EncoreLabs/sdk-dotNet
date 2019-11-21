using System.Collections.Generic;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ApiExceptionTests
    {
        private static readonly object[] SourceForErrorsProperty =
        {
            new object[]
            {
                new List<string> {"Moved"},
                new RestResponse {StatusDescription = "Moved", ErrorMessage = "Resource moved"},
                null
            },
            new object[]
            {
                new List<string> {"Connection was closed"},
                new RestResponse {StatusDescription = null, ErrorMessage = "Connection was closed"},
                null
            },
            new object[]
            {
                new List<string> {"Not found"},
                new RestResponse {StatusDescription = "Not found", ErrorMessage = "404"},
                new Context(),
            },
            new object[]
            {
                new List<string> {"Connection was closed"},
                new RestResponse {StatusDescription = null, ErrorMessage = "Connection was closed"},
                new Context(),
            },
            new object[]
            {
                new List<string>(),
                new RestResponse {StatusDescription = "Not found"},
                new Context {errors = new List<Error>()},
            },
            new object[]
            {
                new List<string> {"Venue [9] is not found"},
                new RestResponse {StatusDescription = "Not found"},
                new Context {errors = new List<Error>
                {
                    new Error {message = "Venue [9] is not found"}
                }},
            },
            new object[]
            {
                new List<string> {"coupon.code - This value should not be blank."},
                new RestResponse {StatusDescription = "OK"},
                new Context {errors = new List<Error>
                {
                    new Error {message = "This value should not be blank.", code = "validation_error", field = "coupon.code"}
                }},
            },
            new object[]
            {
                new List<string> {"Product [9] is not found", "Unauthorized", "coupon.code - This value should not be blank."},
                new RestResponse {StatusDescription = "Not found"},
                new Context
                {
                    errors = new List<Error>
                    {
                        new Error {message = "Product [9] is not found"},
                        new Error {message = "Unauthorized"},
                        new Error {message = "This value should not be blank.", code = "validation_error", field = "coupon.code"}
                    }
                },
            },
        };

        private static readonly object[] SourceForMessagesProperty =
        {
            new object[]
            {
                "Moved",
                new RestResponse {StatusDescription = "Moved", ErrorMessage = "Resource moved"},
                null
            },
            new object[]
            {
                "Connection was closed",
                new RestResponse {StatusDescription = null, ErrorMessage = "Connection was closed"},
                null
            },
            new object[]
            {
                "Not found",
                new RestResponse {StatusDescription = "Not found", ErrorMessage = "404"},
                new Context(),
            },
            new object[]
            {
                "Connection was closed",
                new RestResponse {StatusDescription = null, ErrorMessage = "Connection was closed"},
                new Context(),
            },
            new object[]
            {
                null,
                new RestResponse {StatusDescription = "Not found"},
                new Context {errors = new List<Error>()},
            },
            new object[]
            {
                "Venue [9] is not found",
                new RestResponse {StatusDescription = "Not found"},
                new Context {errors = new List<Error> {new Error {message = "Venue [9] is not found"}}},
            },
            new object[]
            {
                "coupon.code - This value should not be blank.",
                new RestResponse {StatusDescription = "OK"},
                new Context {errors = new List<Error>
                {
                    new Error {message = "This value should not be blank.", code = "validation_error", field = "coupon.code"}
                }},
            },
            new object[]
            {
                "Product [9] is not found; Unauthorized; coupon.code - This value should not be blank.",
                new RestResponse {StatusDescription = "Not found"},
                new Context
                {
                    errors = new List<Error>
                    {
                        new Error {message = "Product [9] is not found"},
                        new Error {message = "Unauthorized"},
                        new Error {message = "This value should not be blank.", code = "validation_error", field = "coupon.code"}
                    }
                },
            },
        };

        private static readonly object[] SourceForDetailsProperty =
        {
            new object[]
            {
                null,
                null
            },
            new object[]
            {
                new Dictionary<string, object>(),
                new Request()
            },
            new object[]
            {
                new Dictionary<string, object>
                {
                    {"body", "{ username : admin, password : pwd }"},
                },
                new Request
                {
                    body = "{ username : admin, password : pwd }"
                }
            },
            new object[]
            {
                new Dictionary<string, object>
                {
                    {"productId", "9"},
                    {"quantity", "2"}
                },
                new Request
                {
                    query = new Dictionary<string, object>
                    {
                        {"productId", "9"},
                        {"quantity", "2"},
                    }
                }
            },
            new object[]
            {
                new Dictionary<string, object>
                {
                    {"promoId", "19"},
                },
                new Request
                {
                    urlParams = new Dictionary<string, object>
                    {
                        {"promoId", "19"},
                    }
                }
            },
            new object[]
            {
                new Dictionary<string, object>
                {
                    {"productId", "9"},
                    {"quantity", "2"},
                    {"promoId", "19"},
                    {"body", "{ username : admin, password : pwd }"},
                },
                new Request
                {
                    query = new Dictionary<string, object>
                    {
                        {"productId", "9"},
                        {"quantity", "2"},
                    },
                    urlParams = new Dictionary<string, object>
                    {
                        {"promoId", "19"},
                    },
                    body = "{ username : admin, password : pwd }"
                }
            },
        };

        [TestCaseSource(nameof(SourceForErrorsProperty))]
        public void Api_ApiException_ErrorsProperty_ReturnsCorrectValue(List<string> expectedErrors, IRestResponse response, Context context)
        {
            var exception =
                new ApiException(response, It.IsAny<ApiContext>(), context, It.IsAny<Request>());

            var result = exception.Errors;

            AssertExtension.EnumerableAreEquals(expectedErrors, result);
        }

        [TestCaseSource(nameof(SourceForMessagesProperty))]
        public void Api_ApiException_MessageProperty_ReturnsCorrectValue(string expected, IRestResponse response, Context context)
        {
            var exception =
                new ApiException(response, It.IsAny<ApiContext>(), context, It.IsAny<Request>());

            var result = exception.Message;

            Assert.AreEqual(expected, result);
        }

        [TestCaseSource(nameof(SourceForDetailsProperty))]
        public void Api_ApiException_DetailsProperty_ReturnsCorrectValue(Dictionary<string, object> expected, Request request)
        {
            var exception =
                new ApiException(It.IsAny<RestResponse>(), It.IsAny<ApiContext>(), It.IsAny<Context>(), request);

            var result = exception.Details;

            AssertExtension.EnumerableAreEquals(expected, result);
        }
    }
}
