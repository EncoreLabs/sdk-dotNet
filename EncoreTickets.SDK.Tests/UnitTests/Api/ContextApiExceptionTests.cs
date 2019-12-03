using System.Collections.Generic;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results;
using EncoreTickets.SDK.Api.Results.Response;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.Tests.Api
{
    internal class ContextApiExceptionTests
    {
        private static readonly object[] SourceForErrorsProperty =
        {
            new object[]
            {
                new List<string>(),
                new RestResponse {StatusDescription = "Moved"},
                null,
                new Info[] { },
            },
            new object[]
            {
                new List<string> {"notValidPromotionCode"},
                new RestResponse {StatusDescription = "Not found"},
                new Context {errors = new List<Error>(), info = new List<Info>()},
                new[] {new Info {code = "notValidPromotionCode"}},
            },
            new object[]
            {
                new List<string> {"notValidPromotionCode"},
                new RestResponse {StatusDescription = "Not found"},
                new Context
                {
                    errors = new List<Error>
                    {
                        new Error {message = "Product [9] is not found"},
                        new Error {message = "Unauthorized"},
                        new Error
                        {
                            message = "This value should not be blank.", code = "validation_error",
                            field = "coupon.code"
                        }
                    }
                },
                new[] {new Info {code = "notValidPromotionCode"}},
            },
            new object[]
            {
                new List<string> {"notValidPromotionCode"},
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    info = new List<Info>
                    {
                        new Info {message = "Some info", code = "someInfo", type = "information"}
                    }
                },
                new[] {new Info {code = "notValidPromotionCode"}},
            },
            new object[]
            {
                new List<string>
                    {"The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code"},
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    info = new List<Info>
                    {
                        new Info
                        {
                            message =
                                "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                            code = "notValidPromotionCode", type = "information", name = "coupon"
                        },
                        new Info {message = "Some info", code = "someInfo", type = "information"}
                    }
                },
                new[]
                {
                    new Info
                    {
                        message =
                            "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                        code = "notValidPromotionCode", type = "information", name = "coupon"
                    }
                },
            },
            new object[]
            {
                new List<string> {"notValidPromotionCode"},
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    info = new List<Info>
                    {
                        new Info {message = "", code = "notValidPromotionCode", type = "information", name = "coupon"}
                    }
                },
                new[]
                {
                    new Info {message = "", code = "notValidPromotionCode", type = "information", name = "coupon"}
                },
            },
            new object[]
            {
                new List<string>
                {
                    "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                    "The warning"
                },
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    info = new List<Info>
                    {
                        new Info
                        {
                            message =
                                "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                            code = "notValidPromotionCode", type = "information", name = "coupon"
                        },
                        new Info {message = "The warning", code = "warning"},
                    }
                },
                new[]
                {
                    new Info
                    {
                        message =
                            "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                        code = "notValidPromotionCode", type = "information", name = "coupon"
                    },
                    new Info {message = "The warning", code = "warning"},
                },
            },
        };

        private static readonly object[] SourceForMessagesProperty =
        {
            new object[]
            {
                "API exception occured",
                new RestResponse {StatusDescription = "Moved"},
                null,
                new Info[] { },
            },
            new object[]
            {
                "notValidPromotionCode",
                new RestResponse {StatusDescription = "Not found"},
                new Context {errors = new List<Error>(), info = new List<Info>()},
                new[] {new Info {code = "notValidPromotionCode"}},
            },
            new object[]
            {
                "notValidPromotionCode",
                new RestResponse {StatusDescription = "Not found"},
                new Context
                {
                    errors = new List<Error>
                    {
                        new Error {message = "Product [9] is not found"},
                        new Error {message = "Unauthorized"},
                        new Error
                        {
                            message = "This value should not be blank.", code = "validation_error",
                            field = "coupon.code"
                        }
                    }
                },
                new[] {new Info {code = "notValidPromotionCode"}},
            },
            new object[]
            {
                "notValidPromotionCode",
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    info = new List<Info>
                    {
                        new Info {message = "Some info", code = "someInfo", type = "information"}
                    }
                },
                new[] {new Info {code = "notValidPromotionCode"}},
            },
            new object[]
            {
                "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    info = new List<Info>
                    {
                        new Info
                        {
                            message =
                                "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                            code = "notValidPromotionCode", type = "information", name = "coupon"
                        }
                    }
                },
                new[]
                {
                    new Info
                    {
                        message =
                            "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                        code = "notValidPromotionCode", type = "information", name = "coupon"
                    }
                },
            },
            new object[]
            {
                "notValidPromotionCode",
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    info = new List<Info>
                    {
                        new Info {message = "", code = "notValidPromotionCode", type = "information", name = "coupon"}
                    }
                },
                new[]
                {
                    new Info {message = "", code = "notValidPromotionCode", type = "information", name = "coupon"}
                }
            },
            new object[]
            {
                "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code; The warning",
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    info = new List<Info>
                    {
                        new Info
                        {
                            message =
                                "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                            code = "notValidPromotionCode", type = "information", name = "coupon"
                        },
                        new Info {message = "The warning", code = "warning"},
                    }
                },
                new[]
                {
                    new Info
                    {
                        message =
                            "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                        code = "notValidPromotionCode", type = "information", name = "coupon"
                    },
                    new Info {message = "The warning", code = "warning"},
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
        public void Api_ContextApiException_ErrorsProperty_ReturnsCorrectValue(List<string> expectedErrors,
            IRestResponse response, Context context, Info[] codesOfInfosAsWarnings)
        {
            var exception = new ContextApiException(codesOfInfosAsWarnings, response, It.IsAny<ApiContext>(), context,
                It.IsAny<Request>());

            var result = exception.Errors;

            AssertExtension.EnumerableAreEquals(expectedErrors, result);
        }

        [TestCaseSource(nameof(SourceForMessagesProperty))]
        public void Api_ContextApiException_MessageProperty_ReturnsCorrectValue(string expected, IRestResponse response,
            Context context, Info[] codesOfInfosAsWarnings)
        {
            var exception = new ContextApiException(codesOfInfosAsWarnings, response, It.IsAny<ApiContext>(), context,
                It.IsAny<Request>());

            var result = exception.Message;

            Assert.AreEqual(expected, result);
        }
    }
}
