using System.Collections.Generic;
using EncoreTickets.SDK.Api.Context;
using EncoreTickets.SDK.Api.Results.Exceptions;
using EncoreTickets.SDK.Api.Results.Response;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace EncoreTickets.SDK.Tests.UnitTests.Api
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
                new Context {Errors = new List<Error>(), Info = new List<Info>()},
                new[] {new Info {Code = "notValidPromotionCode"}},
            },
            new object[]
            {
                new List<string> {"notValidPromotionCode"},
                new RestResponse {StatusDescription = "Not found"},
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error {Message = "Product [9] is not found"},
                        new Error {Message = "Unauthorized"},
                        new Error
                        {
                            Message = "This value should not be blank.", Code = "validation_error",
                            Field = "coupon.code"
                        }
                    }
                },
                new[] {new Info {Code = "notValidPromotionCode"}},
            },
            new object[]
            {
                new List<string> {"notValidPromotionCode"},
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    Info = new List<Info>
                    {
                        new Info {Message = "Some info", Code = "someInfo", Type = "information"}
                    }
                },
                new[] {new Info {Code = "notValidPromotionCode"}},
            },
            new object[]
            {
                new List<string>
                    {"The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code"},
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    Info = new List<Info>
                    {
                        new Info
                        {
                            Message =
                                "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                            Code = "notValidPromotionCode", Type = "information", Name = "coupon"
                        },
                        new Info {Message = "Some info", Code = "someInfo", Type = "information"}
                    }
                },
                new[]
                {
                    new Info
                    {
                        Message =
                            "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                        Code = "notValidPromotionCode", Type = "information", Name = "coupon"
                    }
                },
            },
            new object[]
            {
                new List<string> {"notValidPromotionCode"},
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    Info = new List<Info>
                    {
                        new Info {Message = "", Code = "notValidPromotionCode", Type = "information", Name = "coupon"}
                    }
                },
                new[]
                {
                    new Info {Message = "", Code = "notValidPromotionCode", Type = "information", Name = "coupon"}
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
                    Info = new List<Info>
                    {
                        new Info
                        {
                            Message =
                                "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                            Code = "notValidPromotionCode", Type = "information", Name = "coupon"
                        },
                        new Info {Message = "The warning", Code = "warning"},
                    }
                },
                new[]
                {
                    new Info
                    {
                        Message =
                            "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                        Code = "notValidPromotionCode", Type = "information", Name = "coupon"
                    },
                    new Info {Message = "The warning", Code = "warning"},
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
                new Context {Errors = new List<Error>(), Info = new List<Info>()},
                new[] {new Info {Code = "notValidPromotionCode"}},
            },
            new object[]
            {
                "notValidPromotionCode",
                new RestResponse {StatusDescription = "Not found"},
                new Context
                {
                    Errors = new List<Error>
                    {
                        new Error {Message = "Product [9] is not found"},
                        new Error {Message = "Unauthorized"},
                        new Error
                        {
                            Message = "This value should not be blank.", Code = "validation_error",
                            Field = "coupon.code"
                        }
                    }
                },
                new[] {new Info {Code = "notValidPromotionCode"}},
            },
            new object[]
            {
                "notValidPromotionCode",
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    Info = new List<Info>
                    {
                        new Info {Message = "Some info", Code = "someInfo", Type = "information"}
                    }
                },
                new[] {new Info {Code = "notValidPromotionCode"}},
            },
            new object[]
            {
                "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    Info = new List<Info>
                    {
                        new Info
                        {
                            Message =
                                "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                            Code = "notValidPromotionCode", Type = "information", Name = "coupon"
                        }
                    }
                },
                new[]
                {
                    new Info
                    {
                        Message =
                            "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                        Code = "notValidPromotionCode", Type = "information", Name = "coupon"
                    }
                },
            },
            new object[]
            {
                "notValidPromotionCode",
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    Info = new List<Info>
                    {
                        new Info {Message = "", Code = "notValidPromotionCode", Type = "information", Name = "coupon"}
                    }
                },
                new[]
                {
                    new Info {Message = "", Code = "notValidPromotionCode", Type = "information", Name = "coupon"}
                }
            },
            new object[]
            {
                "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code; The warning",
                new RestResponse {StatusDescription = "OK"},
                new Context
                {
                    Info = new List<Info>
                    {
                        new Info
                        {
                            Message =
                                "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                            Code = "notValidPromotionCode", Type = "information", Name = "coupon"
                        },
                        new Info {Message = "The warning", Code = "warning"},
                    }
                },
                new[]
                {
                    new Info
                    {
                        Message =
                            "The supplied promotion code [TEST] was not applied as it didn't match a valid promotion code",
                        Code = "notValidPromotionCode", Type = "information", Name = "coupon"
                    },
                    new Info {Message = "The warning", Code = "warning"},
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
                    Body = "{ username : admin, password : pwd }"
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
                    Query = new Dictionary<string, object>
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
                    UrlParams = new Dictionary<string, object>
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
                    Query = new Dictionary<string, object>
                    {
                        {"productId", "9"},
                        {"quantity", "2"},
                    },
                    UrlParams = new Dictionary<string, object>
                    {
                        {"promoId", "19"},
                    },
                    Body = "{ username : admin, password : pwd }"
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
